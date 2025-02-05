using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Queries
{
    public class GetSaleByNumberQueryHandler : IRequestHandler<GetSaleByNumberQuery, SaleDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSaleByNumberQueryHandler> _logger;

        public GetSaleByNumberQueryHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            ILogger<GetSaleByNumberQueryHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SaleDto> Handle(GetSaleByNumberQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving sale with number {SaleNumber}", request.SaleNumber);
            var sale = await _saleRepository.GetSaleByNumberAsync(request.SaleNumber);
            if (sale == null)
            {
                throw new KeyNotFoundException($"Sale with number {request.SaleNumber} was not found.");
            }

            var saleDto = _mapper.Map<SaleDto>(sale);
            return saleDto;
        }
    }
}
