using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesCatalog.Infra.Data.Context
{
    public class ContextDb : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly IStoredEventPersistence _storedEventPersistence;

        public ContextDb(
            DbContextOptions<ContextDb> options,
            IDomainEventDispatcher dispatcher,
            IStoredEventPersistence storedEventPersistence)
            : base(options)
        {
            _dispatcher = dispatcher;
            _storedEventPersistence = storedEventPersistence;
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }

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
            {
                var toStore = domainEvents.OfType<IStoredDomainEvent>().ToList();
                if (toStore.Count > 0)
                    await _storedEventPersistence.AppendStoredDomainEventsAsync(toStore, cancellationToken);
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await _dispatcher.DispatchAsync(domainEvents);

            foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
                entry.Entity.ClearDomainEvents();

            return result;
        }
    }
}
