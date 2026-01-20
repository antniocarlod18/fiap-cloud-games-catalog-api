using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGames.Contracts.IntegrationEvents;

public record OrderRefundedIntegrationEvent
{
    public Guid UserId { get; init; }
    public Guid OrderId { get; init; }
    public IList<Guid> GameIds { get; init; }
}
