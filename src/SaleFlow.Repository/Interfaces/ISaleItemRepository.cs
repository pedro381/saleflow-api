using SaleFlow.Domain.Entities;

namespace SaleFlow.Repository.Interfaces
{
    public interface ISaleItemRepository
    {
        Task AddSaleItemAsync(SaleItem saleItem);
        Task UpdateSaleItemAsync(SaleItem saleItem);
        Task DeleteSaleItemAsync(int saleItemId);
        Task<SaleItem?> GetSaleItemByIdAsync(int saleItemId);
        Task<IEnumerable<SaleItem>> GetSaleItemsBySaleNumberAsync(string saleNumber);
    }
}
