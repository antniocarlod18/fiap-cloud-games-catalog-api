using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FiapCloudGamesCatalog.Infra.Data.Context
{
    public class ContextDb : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        public ContextDb(
            DbContextOptions<ContextDb> options,
            IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<StoredEvent> StoredEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDb).Assembly);
        }

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker
                .Entries<AggregateRoot>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            if (domainEvents.Count != 0)
                await AppendStoredEventsAsync(domainEvents, cancellationToken);

            var result = await base.SaveChangesAsync(cancellationToken);

            await _dispatcher.DispatchAsync(domainEvents);

            foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
                entry.Entity.ClearDomainEvents();

            return result;
        }

        private async Task AppendStoredEventsAsync(
            IReadOnlyCollection<Domain.Events.IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            var eventsToStore = domainEvents
                .OfType<Domain.Events.IStoredDomainEvent>()
                .ToList();

            if (eventsToStore.Count == 0)
                return;

            var eventsByAggregate = eventsToStore
                .GroupBy(e => new { e.AggregateId, e.AggregateType })
                .ToList();

            foreach (var group in eventsByAggregate)
            {
                var currentVersion = await StoredEvents
                    .Where(e => e.AggregateId == group.Key.AggregateId)
                    .MaxAsync(e => (int?)e.Version, cancellationToken) ?? 0;

                var nextVersion = currentVersion + 1;

                foreach (var domainEvent in group)
                {
                    var eventType = domainEvent.GetType();
                    var stored = new StoredEvent
                    {
                        AggregateId = group.Key.AggregateId,
                        AggregateType = group.Key.AggregateType,
                        Version = nextVersion++,
                        EventType = eventType.Name,
                        OccurredOn = domainEvent.OccurredOn,
                        Data = JsonSerializer.Serialize(domainEvent, eventType),
                        Metadata = null
                    };

                    await StoredEvents.AddAsync(stored, cancellationToken);
                }
            }
        }
    }
}
