using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Queries
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, IEnumerable<SaleDto>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSalesQueryHandler> _logger;

        public GetSalesQueryHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            ILogger<GetSalesQueryHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all sales");
            var sales = await _saleRepository.GetAllSalesAsync();
            var salesDto = sales.Select(s => _mapper.Map<SaleDto>(s));
            return salesDto;
        }
    }
}
