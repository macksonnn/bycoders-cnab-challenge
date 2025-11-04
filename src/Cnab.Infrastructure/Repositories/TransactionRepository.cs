using Cnab.Application.Interfaces;
using Cnab.Domain.Entities;
using Cnab.Infrastructure.Data;

namespace Cnab.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly CnabDbContext _context;

    public TransactionRepository(CnabDbContext context)
    {
        _context = context;
    }

    public async Task SaveTransactionsAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken = default)
    {
        _context.Transactions.AddRange(transactions);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

