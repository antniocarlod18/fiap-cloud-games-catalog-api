using FiapCloudGamesCatalog.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesCatalog.Domain.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> events);
    }
}
