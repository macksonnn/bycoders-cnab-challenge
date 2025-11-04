using Cnab.Application.DTOs;

namespace Cnab.Application.Interfaces;

public interface IStoreService
{
    Task<List<StoreDto>> GetStoresWithBalanceAsync(CancellationToken cancellationToken = default);
}

