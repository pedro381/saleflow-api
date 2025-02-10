using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.Commands;
using SaleFlow.Service.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SaleFlow.Repository.Tests.Service.Commands;
public class CreateSaleCommandHandlerTest
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateSaleCommandHandler>> _loggerMock;
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTest()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateSaleCommandHandler>>();
        _handler = new CreateSaleCommandHandler(_saleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidSale_CreatesSaleAndReturnsSaleDto()
    {
        var saleDto = new SaleDto
        {
            SaleNumber = "123",
            SaleItems = new List<SaleItemDto>
            {
                new SaleItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 }
            }
        };

        var sale = new Sale { SaleNumber = "123", SaleItems = new List<SaleItem>() };

        _mapperMock.Setup(m => m.Map<Sale>(saleDto)).Returns(sale);
        _mapperMock.Setup(m => m.Map<SaleItem>(It.IsAny<SaleItemDto>())).Returns(new SaleItem());
        _mapperMock.Setup(m => m.Map<SaleDto>(sale)).Returns(saleDto);

        _saleRepositoryMock.Setup(repo => repo.AddSaleAsync(It.IsAny<Sale>())).Returns(Task.CompletedTask);

        var command = new CreateSaleCommand(saleDto);
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(saleDto.SaleNumber, result.SaleNumber);
        _saleRepositoryMock.Verify(repo => repo.AddSaleAsync(It.IsAny<Sale>()), Times.Once);
    }
}
