using MediatR;

namespace SaleFlow.Service.Commands
{
    public class DeleteSaleCommand : IRequest<bool>
    {
        public string SaleNumber { get; }

        public DeleteSaleCommand(string saleNumber)
        {
            SaleNumber = saleNumber;
        }
    }
}
