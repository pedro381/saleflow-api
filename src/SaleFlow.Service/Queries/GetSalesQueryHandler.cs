using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;

namespace SaleFlow.Service.Queries
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, (IEnumerable<SaleDto>, int)>
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

        public async Task<(IEnumerable<SaleDto>, int)> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all sales");
            var (sales, total) = await _saleRepository.GetPagedSalesAsync(request.PageNumber, request.PageSize);
            var salesDto = sales.Select(s => _mapper.Map<SaleDto>(s));
            return (salesDto, total);
        }
    }
}
