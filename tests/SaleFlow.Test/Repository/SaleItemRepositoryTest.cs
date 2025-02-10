using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SaleFlow.Domain.Entities;
using SaleFlow.Repository.Data;
using SaleFlow.Repository.Repositories;
using Xunit;

namespace SaleFlow.Repository.Tests.Repository;

public class GetSaleByNumberQueryHandlerTest
{
    private SaleFlowDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SaleFlowDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new SaleFlowDbContext(options);
    }

    private SaleItem CreateTestSaleItem(int saleItemId, string saleNumber)
    {
        return new SaleItem
        {
            SaleItemId = saleItemId,
        };
    }

    [Fact]
    public async Task AddSaleItemAsync_ShouldAddSaleItem()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleItemRepository>>();
        var repository = new SaleItemRepository(context, loggerMock.Object);

        var saleItem = CreateTestSaleItem(1, "S001");

        // Act
        await repository.AddSaleItemAsync(saleItem);

        // Assert
        var result = await context.SaleItems.FindAsync(1);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateSaleItemAsync_ShouldUpdateSaleItem()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleItemRepository>>();
        var repository = new SaleItemRepository(context, loggerMock.Object);

        var saleItem = CreateTestSaleItem(2, "S002");
        await repository.AddSaleItemAsync(saleItem);

        // Act
        // Exemplo: atualiza o SaleNumber
        await repository.UpdateSaleItemAsync(saleItem);

        // Assert
        var result = await context.SaleItems.FindAsync(2);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteSaleItemAsync_ShouldDeleteSaleItem()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleItemRepository>>();
        var repository = new SaleItemRepository(context, loggerMock.Object);

        var saleItem = CreateTestSaleItem(3, "S003");
        await repository.AddSaleItemAsync(saleItem);

        // Act
        await repository.DeleteSaleItemAsync(3);

        // Assert
        var result = await context.SaleItems.FindAsync(3);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSaleItemAsync_NonExistingItem_ShouldLogWarning()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleItemRepository>>();
        var repository = new SaleItemRepository(context, loggerMock.Object);

        // Act
        await repository.DeleteSaleItemAsync(999);

        // Assert: verifica se o log de Warning foi chamado.
        loggerMock.Verify(l => l.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("999")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetSaleItemByIdAsync_ShouldReturnSaleItem()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleItemRepository>>();
        var repository = new SaleItemRepository(context, loggerMock.Object);

        var saleItem = CreateTestSaleItem(4, "S004");
        await repository.AddSaleItemAsync(saleItem);

        // Act
        var result = await repository.GetSaleItemByIdAsync(4);

        // Assert
        Assert.NotNull(result);
    }
}
