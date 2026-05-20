using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FiapCloudGamesCatalog.Infra.Data.Context;

/// <summary>
/// EF Core design-time factory so migrations work without starting the API host
/// (which may require Elasticsearch, Redis, etc.).
/// </summary>
public sealed class ContextDbDesignTimeFactory : IDesignTimeDbContextFactory<ContextDb>
{
    public ContextDb CreateDbContext(string[] args)
    {
        var apiDirectory = FindApiProjectDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("MySQL")
            ?? throw new InvalidOperationException("Connection string 'MySQL' not found for design-time migrations.");

        var serverVersion = new MySqlServerVersion(new Version(8, 0));
        var optionsBuilder = new DbContextOptionsBuilder<ContextDb>();
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new ContextDb(
            optionsBuilder.Options,
            new DesignTimeNoOpDomainEventDispatcher(),
            new DesignTimeNoOpStoredEventPersistence());
    }

    private static string FindApiProjectDirectory()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, "FiapCloudGamesCatalog.Api", "appsettings.json");
            if (File.Exists(candidate))
                return Path.Combine(dir.FullName, "FiapCloudGamesCatalog.Api");
            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "Could not locate FiapCloudGamesCatalog.Api/appsettings.json. Run dotnet ef from the solution directory.");
    }

    private sealed class DesignTimeNoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(IEnumerable<IDomainEvent> events) => Task.CompletedTask;
    }

    private sealed class DesignTimeNoOpStoredEventPersistence : IStoredEventPersistence
    {
        public Task AppendStoredDomainEventsAsync(
            IReadOnlyList<IStoredDomainEvent> events,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
