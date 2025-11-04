using Cnab.Application.Services;
using Cnab.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Cnab.Tests.Application;

public class CnabParserTests
{
    [Fact]
    public void ParseLine_ValidLine_ShouldParseCorrectly()
    {
        // Arrange
        var line = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       ";

        // Act
        var result = CnabParser.ParseLine(line);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(TransactionType.Financiamento);
        result.OccurredAt.Should().Be(new DateTime(2019, 3, 1, 15, 31, 53));
        result.Value.Should().Be(142.00m);
        result.Cpf.Should().Be("09620676017");
        result.Card.Should().Be("4753****3153");
        result.Owner.Should().Be("JOÃO MACEDO");
        result.StoreName.Should().Be("BAR DO JOÃO");
    }

    [Fact]
    public void ParseLine_AnotherValidLine_ShouldParseCorrectly()
    {
        // Arrange
        var line = "1201903010000023200055641815708231****1231JOSÉ COSTA    MERCEARIA 3 IRMÃOS";

        // Act
        var result = CnabParser.ParseLine(line);

        // Assert
        result.Type.Should().Be(TransactionType.Debito);
        result.OccurredAt.Should().Be(new DateTime(2019, 3, 1, 12, 31, 0));
        result.Value.Should().Be(232.00m);
        result.Cpf.Should().Be("05564181570");
        result.Card.Should().Be("8231****1231");
        result.Owner.Should().Be("JOSÉ COSTA");
        result.StoreName.Should().Be("MERCEARIA 3 IRMÃOS");
    }

    [Fact]
    public void ParseLine_EmptyLine_ShouldThrowException()
    {
        // Arrange
        var line = "";

        // Act
        Action act = () => CnabParser.ParseLine(line);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Linha CNAB não pode ser vazia.*");
    }

    [Fact]
    public void ParseLine_ShortLine_ShouldThrowException()
    {
        // Arrange
        var line = "123456789012345"; // Menos de 81 caracteres

        // Act
        Action act = () => CnabParser.ParseLine(line);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Linha CNAB deve ter no mínimo 81 caracteres.*");
    }

    [Theory]
    [InlineData("1201903010000050000")]
    [InlineData("1201903010000012345")]
    public void ParseLine_ValueParsing_ShouldDivideBy100(string valueInLine)
    {
        // Arrange - Construindo uma linha válida com o valor específico
        var line = $"1{valueInLine}09620676017****3153153453JOÃO MACEDO   BAR DO JOÃO       ";

        // Act
        var result = CnabParser.ParseLine(line);

        // Assert
        var expectedValue = decimal.Parse(valueInLine.Substring(8)) / 100m;
        result.Value.Should().Be(expectedValue);
    }
}

