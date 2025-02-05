using MediatR;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Queries
{
    public class GetSaleByNumberQuery : IRequest<SaleDto>
    {
        public string SaleNumber { get; }

        public GetSaleByNumberQuery(string saleNumber)
        {
            SaleNumber = saleNumber;
        }
    }
}
