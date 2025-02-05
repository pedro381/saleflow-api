using MediatR;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Queries
{
    public class GetSalesQuery : IRequest<IEnumerable<SaleDto>>
    {
        // Optionally add pagination, filtering, and ordering parameters here.
    }
}
