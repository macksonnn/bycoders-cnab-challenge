using Cnab.Application.Interfaces;
using Cnab.Application.Services;
using Cnab.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Text;
using Xunit;

namespace Cnab.Tests.Application;

public class CnabImportServiceTests
{
    private readonly Mock<IStoreRepository> _storeRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly CnabImportService _service;

    public CnabImportServiceTests()
    {
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _service = new CnabImportService(_storeRepositoryMock.Object, _transactionRepositoryMock.Object);
    }

    [Fact]
    public async Task ImportAsync_ValidCnabFile_ShouldCreateStoreAndTransactions()
    {
        // Arrange
        var cnabContent = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \n" +
                          "1201903010000023200055641815708231****1231123100JOSÉ COSTA    MERCEARIA 3 IRMÃOS";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(cnabContent));

        _storeRepositoryMock
            .Setup(r => r.GetByNameAndOwnerAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store?)null);

        _storeRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Store>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store store, CancellationToken ct) =>
            {
                store.Id = 1;
                return store;
            });

        // Act
        await _service.ImportAsync(stream);

        // Assert
        _storeRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Store>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2)); // Duas lojas diferentes

        _transactionRepositoryMock.Verify(
            r => r.SaveTransactionsAsync(It.Is<IEnumerable<Transaction>>(t => t.Count() == 2), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ImportAsync_ExistingStore_ShouldReuseStore()
    {
        // Arrange
        var cnabContent = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \n" +
                          "1201903010000023200055641815708231****1231123100JOÃO MACEDO   BAR DO JOÃO       ";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(cnabContent));

        var existingStore = new Store { Id = 1, Name = "BAR DO JOÃO", Owner = "JOÃO MACEDO" };

        _storeRepositoryMock
            .Setup(r => r.GetByNameAndOwnerAsync("BAR DO JOÃO", "JOÃO MACEDO", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStore);

        // Act
        await _service.ImportAsync(stream);

        // Assert
        _storeRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Store>(), It.IsAny<CancellationToken>()),
            Times.Never); // Não deve criar nova loja

        _transactionRepositoryMock.Verify(
            r => r.SaveTransactionsAsync(It.Is<IEnumerable<Transaction>>(t => t.Count() == 2), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ImportAsync_EmptyFile_ShouldNotCreateAnything()
    {
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(""));

        // Act
        await _service.ImportAsync(stream);

        // Assert
        _storeRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Store>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _transactionRepositoryMock.Verify(
            r => r.SaveTransactionsAsync(It.IsAny<IEnumerable<Transaction>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}

