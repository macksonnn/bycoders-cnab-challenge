using Cnab.Domain.Entities;

namespace Cnab.Application.Interfaces;

public interface IStoreRepository
{
    Task<Store?> GetByNameAndOwnerAsync(string name, string owner, CancellationToken cancellationToken = default);
    Task<Store> CreateAsync(Store store, CancellationToken cancellationToken = default);
    Task<List<Store>> GetAllWithTransactionsAsync(CancellationToken cancellationToken = default);
}

