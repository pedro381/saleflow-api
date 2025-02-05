using Microsoft.EntityFrameworkCore;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Data;
using SaleFlow.Repository.Interfaces;

namespace SaleFlow.Repository.Repositories
{
    public class UpdateSaleCommand : ISaleItemRepository
    {
        private readonly SaleFlowDbContext _context;

        public UpdateSaleCommand(SaleFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddSaleItemAsync(SaleItem saleItem)
        {
            await _context.SaleItems.AddAsync(saleItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSaleItemAsync(SaleItem saleItem)
        {
            _context.SaleItems.Update(saleItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSaleItemAsync(int saleItemId)
        {
            // Assuming a shadow key or an Id property is configured.
            var saleItem = await _context.SaleItems.FindAsync(saleItemId);
            if (saleItem != null)
            {
                _context.SaleItems.Remove(saleItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SaleItem?> GetSaleItemByIdAsync(int saleItemId)
        {
            return await _context.SaleItems.FindAsync(saleItemId);
        }

        public async Task<IEnumerable<SaleItem>> GetSaleItemsBySaleNumberAsync(string saleNumber)
        {
            // Assuming that a foreign key is defined (e.g., SaleNumber) in the SaleItem table.
            return await _context.SaleItems
                .FromSqlInterpolated($"SELECT * FROM \"SaleItems\" WHERE \"SaleNumber\" = {saleNumber}")
                .ToListAsync();
        }
    }
}
