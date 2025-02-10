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

public class UpdateSaleCommandHandlerTest
{
    // Cria um contexto de banco de dados in-memory para os testes
    private SaleFlowDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SaleFlowDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new SaleFlowDbContext(options);
    }

    // Método auxiliar para criar um objeto de Sale para teste.
    private Sale CreateTestSale(string saleNumber = "S001")
    {
        return new Sale
        {
            SaleNumber = saleNumber,
            // Inicialize outras propriedades se necessário.
            SaleItems = new List<SaleItem>()
        };
    }

    [Fact]
    public async Task AddSaleAsync_ShouldAddSale()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleRepository>>();
        var repository = new SaleRepository(context, loggerMock.Object);

        var sale = CreateTestSale("S001");

        // Act
        await repository.AddSaleAsync(sale);

        // Assert
        var result = await context.Sales.FirstOrDefaultAsync(s => s.SaleNumber == "S001");
        Assert.NotNull(result);
        Assert.Equal("S001", result.SaleNumber);
    }

    [Fact]
    public async Task UpdateSaleAsync_ShouldUpdateSale()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleRepository>>();
        var repository = new SaleRepository(context, loggerMock.Object);

        var sale = CreateTestSale("S002");
        sale.SaleItems = [new SaleItem { SaleItemId = 1 }];
        await repository.AddSaleAsync(sale);

        sale = await repository.GetSaleByNumberAsync("S002");

        // Act
        // Exemplo: adicionando um item à venda para atualizar o registro.
        sale.Branch = "asd";
        await repository.UpdateSaleAsync(sale);

        // Assert
        var result = await context.Sales
            .Include(s => s.SaleItems)
            .FirstOrDefaultAsync(s => s.SaleNumber == "S002");
        Assert.NotNull(result);
        Assert.Single(result.SaleItems);
    }

    [Fact]
    public async Task DeleteSaleAsync_ShouldDeleteSale()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleRepository>>();
        var repository = new SaleRepository(context, loggerMock.Object);

        var sale = CreateTestSale("S003");
        await repository.AddSaleAsync(sale);

        // Act
        await repository.DeleteSaleAsync("S003");

        // Assert
        var result = await context.Sales.FirstOrDefaultAsync(s => s.SaleNumber == "S003");
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteSaleAsync_NonExistingSale_ShouldLogWarning()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleRepository>>();
        var repository = new SaleRepository(context, loggerMock.Object);

        // Act
        await repository.DeleteSaleAsync("NonExistingSale");

        // Assert: verifica se o log de Warning foi chamado.
        loggerMock.Verify(l => l.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("NonExistingSale")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetSaleByNumberAsync_ShouldReturnSale()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleRepository>>();
        var repository = new SaleRepository(context, loggerMock.Object);

        var sale = CreateTestSale("S004");
        await repository.AddSaleAsync(sale);

        // Act
        var result = await repository.GetSaleByNumberAsync("S004");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("S004", result?.SaleNumber);
    }

    [Fact]
    public async Task GetPagedSalesAsync_ShouldReturnPagedResults()
    {
        // Arrange
        using var context = CreateDbContext();
        var loggerMock = new Mock<ILogger<SaleRepository>>();
        var repository = new SaleRepository(context, loggerMock.Object);

        // Adiciona 15 registros
        for (int i = 1; i <= 15; i++)
        {
            var sale = CreateTestSale($"S{i:D3}");
            await repository.AddSaleAsync(sale);
        }

        // Act
        int pageNumber = 2;
        int pageSize = 5;
        var (sales, totalRecords) = await repository.GetPagedSalesAsync(pageNumber, pageSize);

        // Assert
        Assert.Equal(15, totalRecords);
        Assert.Equal(pageSize, sales.Count());
        // Verifica se o primeiro item da página é o esperado (dependendo da ordem inserida)
        var expectedSaleNumber = $"S{(pageNumber - 1) * pageSize + 1:D3}";
        Assert.Equal(expectedSaleNumber, sales.First().SaleNumber);
    }
}
