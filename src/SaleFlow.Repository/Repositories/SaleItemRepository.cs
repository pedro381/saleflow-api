using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Data;
using SaleFlow.Repository.Interfaces;

namespace SaleFlow.Repository.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly SaleFlowDbContext _context;
        private readonly ILogger<SaleItemRepository> _logger;

        public SaleItemRepository(SaleFlowDbContext context, ILogger<SaleItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddSaleItemAsync(SaleItem saleItem)
        {
            try
            {
                await _context.SaleItems.AddAsync(saleItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding sale item with ID {SaleItemId}", saleItem.SaleItemId);
                throw;
            }
        }

        public async Task UpdateSaleItemAsync(SaleItem saleItem)
        {
            try
            {
                _context.SaleItems.Update(saleItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sale item with ID {SaleItemId}", saleItem.SaleItemId);
                throw;
            }
        }

        public async Task DeleteSaleItemAsync(int saleItemId)
        {
            try
            {
                var saleItem = await _context.SaleItems.FindAsync(saleItemId);
                if (saleItem != null)
                {
                    _context.SaleItems.Remove(saleItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Sale item with ID {SaleItemId} not found for deletion", saleItemId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sale item with ID {SaleItemId}", saleItemId);
                throw;
            }
        }

        public async Task<SaleItem?> GetSaleItemByIdAsync(int saleItemId)
        {
            try
            {
                return await _context.SaleItems.FindAsync(saleItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sale item with ID {SaleItemId}", saleItemId);
                throw;
            }
        }

        public async Task<IEnumerable<SaleItem>> GetSaleItemsBySaleNumberAsync(string saleNumber)
        {
            try
            {
                return await _context.SaleItems
                    .FromSqlInterpolated($"SELECT * FROM \"SaleItems\" WHERE \"SaleNumber\" = {saleNumber}")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sale items for sale number {SaleNumber}", saleNumber);
                throw;
            }
        }

    }
}
