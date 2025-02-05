using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Commands
{
    public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, SaleDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleCommandHandler> _logger;

        public UpdateSaleCommandHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            ILogger<UpdateSaleCommandHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SaleDto> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the existing sale from the repository.
            var existingSale = await _saleRepository.GetSaleByNumberAsync(request.SaleDto.SaleNumber);
            if (existingSale == null)
            {
                // Depending on your error-handling strategy, you can throw an exception here.
                throw new KeyNotFoundException($"Sale with number {request.SaleDto.SaleNumber} was not found.");
            }

            // For this example, update the branch and date.
            // (Updating sale items or customer details might require additional business rules.)
            // Note: In a more complete implementation, you would have methods on the domain entity to update state.
            existingSale = _mapper.Map<Sale>(request.SaleDto);

            _logger.LogInformation("Updating sale with number {SaleNumber}", existingSale.SaleNumber);
            await _saleRepository.UpdateSaleAsync(existingSale);

            var updatedSaleDto = _mapper.Map<SaleDto>(existingSale);
            return updatedSaleDto;
        }
    }
}
