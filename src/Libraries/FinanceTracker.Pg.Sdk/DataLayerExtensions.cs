using FinanceTracker.Domain.Entities;
using FinanceTracker.Pg.Sdk.Repositories.Interfaces;
using FinanceTracker.Pg.Sdk.Repositories.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace FinanceTracker.Pg.Sdk;

public static class DataLayerExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services,
        IConfiguration configuration, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString(connectionString));
        dataSourceBuilder.EnableDynamicJson();
        var npgsqlDataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>((_, options) =>
        {
            options.UseNpgsql(npgsqlDataSource,
                    opts => { opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); })
                .EnableSensitiveDataLogging();
        }, serviceLifetime);
        
        
        services.AddScoped<IPgRepository<Ingestion>, PgRepository<Ingestion>>();
        services.AddScoped<IPgRepository<Transaction>, PgRepository<Transaction>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
