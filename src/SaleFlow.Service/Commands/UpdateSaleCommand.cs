using MediatR;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Commands
{
    public class UpdateSaleCommand : IRequest<SaleDto>
    {
        public SaleDto SaleDto { get; }

        public UpdateSaleCommand(SaleDto saleDto)
        {
            SaleDto = saleDto;
        }
    }
}
