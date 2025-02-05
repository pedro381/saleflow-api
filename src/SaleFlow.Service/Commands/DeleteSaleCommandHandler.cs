using MediatR;
using Microsoft.Extensions.Logging;
using SaleFlow.Repository.Interfaces;

namespace SaleFlow.Service.Commands
{
    public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<DeleteSaleCommandHandler> _logger;

        public DeleteSaleCommandHandler(ISaleRepository saleRepository, ILogger<DeleteSaleCommandHandler> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting sale with number {SaleNumber}", request.SaleNumber);
            await _saleRepository.DeleteSaleAsync(request.SaleNumber);
            // For simplicity, return true. In a real scenario, you may check whether deletion was successful.
            return true;
        }
    }
}
