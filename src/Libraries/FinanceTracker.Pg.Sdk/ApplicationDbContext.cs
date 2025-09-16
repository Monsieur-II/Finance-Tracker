using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Pg.Sdk;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Ingestion> Ingestions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
