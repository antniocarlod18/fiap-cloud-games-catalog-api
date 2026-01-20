using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesCatalog.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
