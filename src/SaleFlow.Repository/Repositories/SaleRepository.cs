using Microsoft.EntityFrameworkCore;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Data;
using SaleFlow.Repository.Interfaces;

namespace SaleFlow.Repository.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SaleFlowDbContext _context;

        public SaleRepository(SaleFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddSaleAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSaleAsync(string saleNumber)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Sale?> GetSaleByNumberAsync(string saleNumber)
        {
            // Include SaleItems if needed using .Include(...)
            return await _context.Sales
                .Include(s => s.SaleItems)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Include(s => s.SaleItems)
                .ToListAsync();
        }
    }
}
