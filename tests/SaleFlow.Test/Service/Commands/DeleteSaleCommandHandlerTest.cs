using Microsoft.Extensions.Logging;
using Moq;
using SaleFlow.Repository.Interfaces;
using SaleFlow.Service.Commands;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SaleFlow.Repository.Tests.Service.Commands;
public class DeleteSaleCommandHandlerTest
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<ILogger<DeleteSaleCommandHandler>> _loggerMock;
    private readonly DeleteSaleCommandHandler _handler;

    public DeleteSaleCommandHandlerTest()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _loggerMock = new Mock<ILogger<DeleteSaleCommandHandler>>();
        _handler = new DeleteSaleCommandHandler(_saleRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidSaleNumber_DeletesSaleAndReturnsTrue()
    {
        var saleNumber = "123";

        _saleRepositoryMock.Setup(repo => repo.DeleteSaleAsync(saleNumber))
            .Returns(Task.CompletedTask);

        var command = new DeleteSaleCommand(saleNumber);
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _saleRepositoryMock.Verify(repo => repo.DeleteSaleAsync(saleNumber), Times.Once);
    }
}
