using Cnab.Application.DTOs;
using Cnab.Application.Interfaces;
using Cnab.Domain.Extensions;

namespace Cnab.Application.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;

    public StoreService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<List<StoreDto>> GetStoresWithBalanceAsync(CancellationToken cancellationToken = default)
    {
        var stores = await _storeRepository.GetAllWithTransactionsAsync(cancellationToken);

        return stores.Select(store =>
        {
            var transactions = store.Transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Type = (int)t.Type,
                TypeDescription = t.Type.ToString(),
                OccurredAt = t.OccurredAt,
                Value = t.Value,
                SignedValue = t.Value * t.Type.GetSign(),
                Cpf = t.Cpf,
                Card = t.Card
            }).OrderBy(t => t.OccurredAt).ToList();

            var balance = transactions.Sum(t => t.SignedValue);

            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Owner = store.Owner,
                Balance = balance,
                Transactions = transactions
            };
        }).OrderBy(s => s.Name).ToList();
    }
}

