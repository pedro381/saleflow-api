using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Data;
using SaleFlow.Repository.Interfaces;

namespace SaleFlow.Repository.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly SaleFlowDbContext _context;
    private readonly ILogger<SaleRepository> _logger;

    public SaleRepository(SaleFlowDbContext context, ILogger<SaleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddSaleAsync(Sale sale)
    {
        try
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding sale {SaleNumber}", sale.SaleNumber);
            throw;
        }
    }

    public async Task UpdateSaleAsync(Sale sale)
    {
        try
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sale {SaleNumber}", sale.SaleNumber);
            throw;
        }
    }

    public async Task DeleteSaleAsync(string saleNumber)
    {
        try
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Sale with number {SaleNumber} not found", saleNumber);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sale {SaleNumber}", saleNumber);
            throw;
        }
    }

    public async Task<Sale?> GetSaleByNumberAsync(string saleNumber)
    {
        try
        {
            return await _context.Sales
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sale {SaleNumber}", saleNumber);
            throw;
        }
    }

    public async Task<(IEnumerable<Sale>, int)> GetPagedSalesAsync(int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Sales.Include(s => s.SaleItems);
            var totalRecords = await query.CountAsync();
            var sales = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (sales, totalRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged sales - Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);
            throw;
        }
    }
}
