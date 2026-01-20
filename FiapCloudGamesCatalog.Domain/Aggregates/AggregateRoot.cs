using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Events;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Aggregates
{
    public abstract class AggregateRoot : EntityBase
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        [SetsRequiredMembers]
        protected AggregateRoot() : base()
        {
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
