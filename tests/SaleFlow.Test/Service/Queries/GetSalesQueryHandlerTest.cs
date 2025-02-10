using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;
using SaleFlow.Service.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SaleFlow.Repository.Tests.Service.Queries;
public class GetSalesQueryHandlerTest
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetSalesQueryHandler>> _loggerMock;
    private readonly GetSalesQueryHandler _handler;

    public GetSalesQueryHandlerTest()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<GetSalesQueryHandler>>();
        _handler = new GetSalesQueryHandler(_saleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_SalesExist_ReturnsSalesDtoWithTotal()
    {
        var query = new GetSalesQuery(1, 10);
        var sales = new List<Sale> { new Sale { SaleNumber = "123" } };
        var salesDto = new List<SaleDto> { new SaleDto { SaleNumber = "123" } };

        _saleRepositoryMock.Setup(repo => repo.GetPagedSalesAsync(query.PageNumber, query.PageSize))
            .ReturnsAsync((sales, sales.Count));

        _mapperMock.Setup(mapper => mapper.Map<SaleDto>(It.IsAny<Sale>()))
            .Returns((Sale sale) => new SaleDto { SaleNumber = sale.SaleNumber });

        var (result, total) = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(sales.Count, total);
    }

    [Fact]
    public async Task Handle_NoSales_ReturnsEmptyListWithZeroTotal()
    {
        var query = new GetSalesQuery(1, 10);
        var sales = new List<Sale>();

        _saleRepositoryMock.Setup(repo => repo.GetPagedSalesAsync(query.PageNumber, query.PageSize))
            .ReturnsAsync((sales, sales.Count));

        var (result, total) = await _handler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
        Assert.Equal(0, total);
    }
}
