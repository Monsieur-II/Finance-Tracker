using System.Reflection;
using System.Text.RegularExpressions;
using Akka.Actor;
using Akka.Hosting;
using FinanceTracker.Api.Actors;
using FinanceTracker.Pg.Sdk;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddActorSystem(
        this IServiceCollection services)
    {
        var actorSystemName = Regex.Replace(Assembly.GetExecutingAssembly().GetName().Name ?? "ActorSystemName", @"[^a-zA-Z\s]+", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        
        services.AddAkka(actorSystemName, (builder) =>
        {
            builder.WithActors((system, registry, resolver) =>
            {
                var defaultStrategy = new OneForOneStrategy(
                    3, TimeSpan.FromSeconds(3), ex =>
                    {
                        if (ex is not ActorInitializationException)
                            return Directive.Resume;

                        system?.Terminate().Wait(1000);

                        return Directive.Stop;
                    });

                var actorProps = resolver
                    .Props<FinanceTrackerActor>()
                    .WithSupervisorStrategy(defaultStrategy);

                var mainActor = system.ActorOf(actorProps, nameof(FinanceTrackerActor));
                registry.Register<FinanceTrackerActor>(mainActor);

            });

        });

        return services;
    }
    
    public static async Task ApplyPendingMigrations(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var scopedServiceProvider = scope.ServiceProvider;
        var context = scopedServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.EnsureCreatedAsync();

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}
