using FiapCloudGamesCatalog.Domain.Abstractions;
using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

            var result = await base.SaveChangesAsync(cancellationToken);

            await _dispatcher.DispatchAsync(domainEvents);

            foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
                entry.Entity.ClearDomainEvents();

            return result;
        }
    }
}
