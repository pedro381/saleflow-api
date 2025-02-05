using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Commands
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSaleCommandHandler> _logger;

        public CreateSaleCommandHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            ILogger<CreateSaleCommandHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Map incoming DTO to domain entity
            var saleDto = request.SaleDto;
            var sale = _mapper.Map<Sale>(saleDto);

            // Map and add each sale item.
            foreach (var itemDto in saleDto.SaleItems)
            {
                // The SaleItem constructor applies the discount logic and quantity rules.
                var saleItem = _mapper.Map<SaleItem>(itemDto);
                sale.AddSaleItem(saleItem);
            }

            // Log the creation event.
            _logger.LogInformation("Creating sale with number {SaleNumber}", sale.SaleNumber);

            // Persist the sale via repository.
            await _saleRepository.AddSaleAsync(sale);

            // Optionally, map the created sale back to a DTO for response.
            var createdSaleDto = _mapper.Map<SaleDto>(sale);
            return createdSaleDto;
        }
    }
}
