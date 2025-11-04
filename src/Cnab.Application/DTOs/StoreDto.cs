namespace Cnab.Application.DTOs;

public class StoreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public List<TransactionDto> Transactions { get; set; } = new();
}

