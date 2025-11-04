using Cnab.Application.Interfaces;
using Cnab.Application.Services;
using Cnab.Domain.Entities;
using Cnab.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cnab.Tests.Application;

public class StoreServiceTests
{
    private readonly Mock<IStoreRepository> _storeRepositoryMock;
    private readonly StoreService _service;

    public StoreServiceTests()
    {
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _service = new StoreService(_storeRepositoryMock.Object);
    }

    [Fact]
    public async Task GetStoresWithBalanceAsync_ShouldCalculateBalanceCorrectly()
    {
        var store = new Store
        {
            Id = 1,
            Name = "Loja Teste",
            Owner = "Dono Teste",
            Transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Type = TransactionType.Debito, Value = 100.00m, OccurredAt = DateTime.Now, Cpf = "123", Card = "456", StoreId = 1 },
                new Transaction { Id = 2, Type = TransactionType.Boleto, Value = 50.00m, OccurredAt = DateTime.Now, Cpf = "123", Card = "456", StoreId = 1 },
                new Transaction { Id = 3, Type = TransactionType.Credito, Value = 75.00m, OccurredAt = DateTime.Now, Cpf = "123", Card = "456", StoreId = 1 }
            }
        };

        _storeRepositoryMock
            .Setup(r => r.GetAllWithTransactionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Store> { store });

        var result = await _service.GetStoresWithBalanceAsync();

        result.Should().HaveCount(1);
        result[0].Balance.Should().Be(125.00m);
        result[0].Transactions.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetStoresWithBalanceAsync_EmptyDatabase_ShouldReturnEmptyList()
    {
        _storeRepositoryMock
            .Setup(r => r.GetAllWithTransactionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Store>());

        var result = await _service.GetStoresWithBalanceAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStoresWithBalanceAsync_MultipleStores_ShouldReturnAllStores()
    {
        var stores = new List<Store>
        {
            new Store
            {
                Id = 1,
                Name = "Loja A",
                Owner = "Dono A",
                Transactions = new List<Transaction>
                {
                    new Transaction { Id = 1, Type = TransactionType.Vendas, Value = 200.00m, OccurredAt = DateTime.Now, Cpf = "123", Card = "456", StoreId = 1 }
                }
            },
            new Store
            {
                Id = 2,
                Name = "Loja B",
                Owner = "Dono B",
                Transactions = new List<Transaction>
                {
                    new Transaction { Id = 2, Type = TransactionType.Aluguel, Value = 300.00m, OccurredAt = DateTime.Now, Cpf = "789", Card = "012", StoreId = 2 }
                }
            }
        };

        _storeRepositoryMock
            .Setup(r => r.GetAllWithTransactionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stores);

        var result = await _service.GetStoresWithBalanceAsync();

        result.Should().HaveCount(2);
        result[0].Balance.Should().Be(200.00m);
        result[1].Balance.Should().Be(-300.00m);
    }
}

