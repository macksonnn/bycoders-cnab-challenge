using Cnab.Domain.Entities;

namespace Cnab.Application.Interfaces;

public interface ITransactionRepository
{
    Task SaveTransactionsAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken = default);
}

