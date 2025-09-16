using FinanceTracker.Domain.Entities;
using FinanceTracker.Pg.Sdk.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Pg.Sdk.Repositories.Providers;

public class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    private IPgRepository<Ingestion>? _ingestionsRepo;
    private IPgRepository<Transaction>? _transactionsRepo;

    public IPgRepository<Ingestion> Ingestions => _ingestionsRepo ??= serviceProvider.GetRequiredService<IPgRepository<Ingestion>>();
    public IPgRepository<Transaction> Transactions => _transactionsRepo ??= serviceProvider.GetRequiredService<IPgRepository<Transaction>>();
}
