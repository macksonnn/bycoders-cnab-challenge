using Cnab.Domain.Entities;
using Cnab.Domain.Enums;
using System.Globalization;

namespace Cnab.Application.Services;

public static class CnabParser
{
    public static CnabLineData ParseLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentException("Linha CNAB não pode ser vazia.", nameof(line));

        if (line.Length < 81)
            throw new ArgumentException($"Linha CNAB deve ter no mínimo 81 caracteres. Recebido: {line.Length}", nameof(line));

        var type = int.Parse(line.Substring(0, 1));
        var date = line.Substring(1, 8);
        var valueStr = line.Substring(9, 10);
        var cpf = line.Substring(19, 11).Trim();
        var card = line.Substring(30, 12).Trim();
        var time = line.Substring(42, 6);
        var owner = line.Substring(48, 14).Trim();
        var storeName = line.Substring(62, 19).Trim();

        // Parse da data e hora
        var year = int.Parse(date.Substring(0, 4));
        var month = int.Parse(date.Substring(4, 2));
        var day = int.Parse(date.Substring(6, 2));
        var hour = int.Parse(time.Substring(0, 2));
        var minute = int.Parse(time.Substring(2, 2));
        var second = int.Parse(time.Substring(4, 2));

        var occurredAt = new DateTime(year, month, day, hour, minute, second);
        var value = decimal.Parse(valueStr) / 100m;

        return new CnabLineData
        {
            Type = (TransactionType)type,
            OccurredAt = occurredAt,
            Value = value,
            Cpf = cpf,
            Card = card,
            Owner = owner,
            StoreName = storeName
        };
    }
}

public class CnabLineData
{
    public TransactionType Type { get; set; }
    public DateTime OccurredAt { get; set; }
    public decimal Value { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Card { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
}

