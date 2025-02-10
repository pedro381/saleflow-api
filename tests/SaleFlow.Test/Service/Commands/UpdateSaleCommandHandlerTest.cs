using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.Commands;
using SaleFlow.Service.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SaleFlow.Repository.Tests.Service.Commands;
public class UpdateSaleCommandHandlerTest
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UpdateSaleCommandHandler>> _loggerMock;
    private readonly UpdateSaleCommandHandler _handler;

    public UpdateSaleCommandHandlerTest()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UpdateSaleCommandHandler>>();
        _handler = new UpdateSaleCommandHandler(_saleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingSale_UpdatesAndReturnsSaleDto()
    {
        var saleDto = new SaleDto { SaleNumber = "123" };
        var existingSale = new Sale { SaleNumber = "123" };
        var updatedSale = new Sale { SaleNumber = "123" };

        _saleRepositoryMock.Setup(repo => repo.GetSaleByNumberAsync(saleDto.SaleNumber))
            .ReturnsAsync(existingSale);

        _mapperMock.Setup(mapper => mapper.Map<Sale>(saleDto))
            .Returns(updatedSale);

        _saleRepositoryMock.Setup(repo => repo.UpdateSaleAsync(updatedSale))
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(mapper => mapper.Map<SaleDto>(updatedSale))
            .Returns(saleDto);

        var command = new UpdateSaleCommand(saleDto);
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(saleDto.SaleNumber, result.SaleNumber);
    }

    [Fact]
    public async Task Handle_NonExistingSale_ThrowsKeyNotFoundException()
    {
        var saleDto = new SaleDto { SaleNumber = "999" };

        _saleRepositoryMock.Setup(repo => repo.GetSaleByNumberAsync(saleDto.SaleNumber))
            .ReturnsAsync((Sale)null);

        var command = new UpdateSaleCommand(saleDto);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
