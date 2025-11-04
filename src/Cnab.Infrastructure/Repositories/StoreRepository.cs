using Cnab.Application.Interfaces;
using Cnab.Domain.Entities;
using Cnab.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Infrastructure.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly CnabDbContext _context;

    public StoreRepository(CnabDbContext context)
    {
        _context = context;
    }

    public async Task<Store?> GetByNameAndOwnerAsync(string name, string owner, CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .FirstOrDefaultAsync(s => s.Name == name && s.Owner == owner, cancellationToken);
    }

    public async Task<Store> CreateAsync(Store store, CancellationToken cancellationToken = default)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync(cancellationToken);
        return store;
    }

    public async Task<List<Store>> GetAllWithTransactionsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stores
            .Include(s => s.Transactions)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

