using MediatR;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Commands
{
    public class CreateSaleCommand : IRequest<SaleDto>
    {
        public SaleDto SaleDto { get; }

        public CreateSaleCommand(SaleDto saleDto)
        {
            SaleDto = saleDto;
        }
    }
}
