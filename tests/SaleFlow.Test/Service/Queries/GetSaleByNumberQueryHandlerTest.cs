using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.DTOs;
using SaleFlow.Service.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SaleFlow.Repository.Tests.Service.Queries;
public class GetSaleByNumberQueryHandlerTest
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetSaleByNumberQueryHandler>> _loggerMock;
    private readonly GetSaleByNumberQueryHandler _handler;

    public GetSaleByNumberQueryHandlerTest()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<GetSaleByNumberQueryHandler>>();
        _handler = new GetSaleByNumberQueryHandler(_saleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_SaleExists_ReturnsSaleDto()
    {
        // Arrange
        var saleNumber = "12345";
        var query = new GetSaleByNumberQuery(saleNumber);
        var saleEntity = new Sale { SaleNumber = saleNumber }; // Substitua pelo seu modelo de domínio real
        var saleDto = new SaleDto { SaleNumber = saleNumber };

        _saleRepositoryMock.Setup(repo => repo.GetSaleByNumberAsync(saleNumber))
            .ReturnsAsync(saleEntity);

        _mapperMock.Setup(mapper => mapper.Map<SaleDto>(saleEntity))
            .Returns(saleDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(saleNumber, result.SaleNumber);
    }

    [Fact]
    public async Task Handle_SaleDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var saleNumber = "12345";
        var query = new GetSaleByNumberQuery(saleNumber);

        _ = _saleRepositoryMock.Setup(repo => repo.GetSaleByNumberAsync(saleNumber))
            .ReturnsAsync((Sale?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}