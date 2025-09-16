using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Pg.Sdk.Repositories.Interfaces;

public interface IUnitOfWork
{
    public IPgRepository<Ingestion> Ingestions { get; }
    public IPgRepository<Transaction> Transactions { get; }
}
