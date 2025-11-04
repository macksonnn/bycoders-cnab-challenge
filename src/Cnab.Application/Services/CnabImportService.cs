using Cnab.Application.Interfaces;
using Cnab.Domain.Entities;

namespace Cnab.Application.Services;

public class CnabImportService : ICnabImportService
{
    private readonly IStoreRepository _storeRepository;
    private readonly ITransactionRepository _transactionRepository;

    public CnabImportService(
        IStoreRepository storeRepository,
        ITransactionRepository transactionRepository)
    {
        _storeRepository = storeRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task ImportAsync(Stream cnabStream, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(cnabStream);
        var transactions = new List<Transaction>();
        var storesCache = new Dictionary<(string name, string owner), Store>();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var data = CnabParser.ParseLine(line);
            var storeKey = (data.StoreName, data.Owner);
            
            if (!storesCache.TryGetValue(storeKey, out var store))
            {
                store = await _storeRepository.GetByNameAndOwnerAsync(
                    data.StoreName, 
                    data.Owner, 
                    cancellationToken);

                if (store == null)
                {
                    store = new Store
                    {
                        Name = data.StoreName,
                        Owner = data.Owner
                    };
                    store = await _storeRepository.CreateAsync(store, cancellationToken);
                }

                storesCache[storeKey] = store;
            }

            var transaction = new Transaction
            {
                Type = data.Type,
                OccurredAt = data.OccurredAt,
                Value = data.Value,
                Cpf = data.Cpf,
                Card = data.Card,
                StoreId = store.Id
            };

            transactions.Add(transaction);
        }

        if (transactions.Any())
        {
            await _transactionRepository.SaveTransactionsAsync(transactions, cancellationToken);
        }
    }
}

