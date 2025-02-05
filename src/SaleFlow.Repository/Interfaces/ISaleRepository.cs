using SaleFlow.Domain.Entities;

namespace SaleFlow.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task AddSaleAsync(Sale sale);
        Task UpdateSaleAsync(Sale sale);
        Task DeleteSaleAsync(string saleNumber);
        Task<Sale?> GetSaleByNumberAsync(string saleNumber);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        // Optionally, add methods to support filtering, pagination, etc.
    }
}
