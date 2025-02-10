using MediatR;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Queries
{
    public class GetSalesQuery : IRequest<(IEnumerable<SaleDto>, int)>
    {
        public GetSalesQuery()
        {
        }

        public GetSalesQuery(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
