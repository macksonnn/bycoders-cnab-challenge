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
        var line = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO        ";

        var result = CnabParser.ParseLine(line);

        result.Should().NotBeNull();
        result.Type.Should().Be(TransactionType.Financiamento);
        result.OccurredAt.Should().Be(new DateTime(2019, 3, 1, 15, 34, 53));
        result.Value.Should().Be(142.00m);
        result.Cpf.Should().Be("09620676017");
        result.Card.Should().Be("4753****3153");
        result.Owner.Should().Be("JOÃO MACEDO");
        result.StoreName.Should().Be("BAR DO JOÃO");
    }

    [Fact]
    public void ParseLine_AnotherValidLine_ShouldParseCorrectly()
    {
        var line = "1201903010000023200055641815708231****1231123100JOSÉ COSTA    MERCEARIA 3 IRMÃOS ";

        var result = CnabParser.ParseLine(line);

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
        var line = "";

        Action act = () => CnabParser.ParseLine(line);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Linha CNAB não pode ser vazia.*");
    }

    [Fact]
    public void ParseLine_ShortLine_ShouldThrowException()
    {
        var line = "123456789012345";

        Action act = () => CnabParser.ParseLine(line);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Linha CNAB deve ter no mínimo 81 caracteres.*");
    }

    [Fact]
    public void ParseLine_ValueDivisionBy100_ShouldWork()
    {
        var line = "1201903010000050000096206760174753****3153123000JOÃO MACEDO   BAR DO JOÃO        ";

        var result = CnabParser.ParseLine(line);

        result.Value.Should().Be(500.00m);
    }
}

