namespace Cnab.Application.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string TypeDescription { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
    public decimal Value { get; set; }
    public decimal SignedValue { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Card { get; set; } = string.Empty;
}

