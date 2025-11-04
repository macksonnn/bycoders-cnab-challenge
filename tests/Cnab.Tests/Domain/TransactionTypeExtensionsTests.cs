using Cnab.Domain.Enums;
using Cnab.Domain.Extensions;
using FluentAssertions;
using Xunit;

namespace Cnab.Tests.Domain;

public class TransactionTypeExtensionsTests
{
    [Theory]
    [InlineData(TransactionType.Debito, true)]
    [InlineData(TransactionType.Credito, true)]
    [InlineData(TransactionType.RecebimentoEmprestimo, true)]
    [InlineData(TransactionType.Vendas, true)]
    [InlineData(TransactionType.RecebimentoTed, true)]
    [InlineData(TransactionType.RecebimentoDoc, true)]
    [InlineData(TransactionType.Boleto, false)]
    [InlineData(TransactionType.Financiamento, false)]
    [InlineData(TransactionType.Aluguel, false)]
    public void IsRevenue_ShouldReturnCorrectValue(TransactionType type, bool expectedIsRevenue)
    {
        // Act
        var result = type.IsRevenue();

        // Assert
        result.Should().Be(expectedIsRevenue);
    }

    [Theory]
    [InlineData(TransactionType.Debito, false)]
    [InlineData(TransactionType.Credito, false)]
    [InlineData(TransactionType.Boleto, true)]
    [InlineData(TransactionType.Financiamento, true)]
    [InlineData(TransactionType.Aluguel, true)]
    public void IsExpense_ShouldReturnCorrectValue(TransactionType type, bool expectedIsExpense)
    {
        // Act
        var result = type.IsExpense();

        // Assert
        result.Should().Be(expectedIsExpense);
    }

    [Theory]
    [InlineData(TransactionType.Debito, 1)]
    [InlineData(TransactionType.Credito, 1)]
    [InlineData(TransactionType.Vendas, 1)]
    [InlineData(TransactionType.Boleto, -1)]
    [InlineData(TransactionType.Financiamento, -1)]
    [InlineData(TransactionType.Aluguel, -1)]
    public void GetSign_ShouldReturnCorrectValue(TransactionType type, int expectedSign)
    {
        // Act
        var result = type.GetSign();

        // Assert
        result.Should().Be(expectedSign);
    }
}

