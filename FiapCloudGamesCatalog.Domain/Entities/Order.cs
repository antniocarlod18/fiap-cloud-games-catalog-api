using FiapCloudGamesCatalog.Domain.Aggregates;
using FiapCloudGamesCatalog.Domain.Enums;
using FiapCloudGamesCatalog.Domain.Events;
using FiapCloudGamesCatalog.Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesCatalog.Domain.Entities;

public class Order : AggregateRoot
{
    const int RefundPeriodInDays = 30;

    public required Guid UserId { get; set; }
    public required IList<OrderGameItem> Games { get; set; }
    public decimal Price { get; private set; }
    public required OrderStatusEnum Status { get; set; }

    [SetsRequiredMembers]
    public Order(Guid userId, IList<Game> games) : base()
    {
        if(!games.Any()) 
            throw new CannotCreateAnOrderWithoutItemsException();

        UserId = userId;
        Games = games.Select(g => new OrderGameItem(this, g)).ToList(); ;
        Status = OrderStatusEnum.WaitingForPayment;
        Price = Games.Sum(g => g.Price);
        AddDomainEvent(new OrderCreatedDomainEvent(Id, UserId, GetGameIds(), Price));
    }

    [SetsRequiredMembers]
    public Order()
    {
    }

    public void CompletedOrder()
    {
        if(Status != OrderStatusEnum.WaitingForPayment)
            throw new InvalidOrderStatusException(Status);

        var @event = new OrderCompleteDomainEvent(Id, UserId, GetGameIds());
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void CancelOrder()
    {
        if (Status != OrderStatusEnum.WaitingForPayment)
            throw new InvalidOrderStatusException(Status);

        var @event = new OrderCanceledDomainEvent(Id, UserId, GetGameIds());
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void RefundOrder()
    {
        if(Status == OrderStatusEnum.Completed && DateTime.Now < base.DateCreated.AddDays(RefundPeriodInDays))
        {
            var @event = new OrderRefundedDomainEvent(Id, UserId, GetGameIds());
            Apply(@event);
            AddDomainEvent(@event);
        }
        else
        {
            throw new CannotRefundOrderException();
        }
    }

    private IReadOnlyList<Guid> GetGameIds()
        => Games?.Select(g => g.Game.Id).ToList() ?? [];

    private void Apply(OrderCompleteDomainEvent @event)
    {
        Status = OrderStatusEnum.Completed;
        DateUpdated = DateTime.UtcNow;
    }

    private void Apply(OrderCanceledDomainEvent @event)
    {
        Status = OrderStatusEnum.Canceled;
        DateUpdated = DateTime.UtcNow;
    }

    private void Apply(OrderRefundedDomainEvent @event)
    {
        Status = OrderStatusEnum.Refunded;
        DateUpdated = DateTime.UtcNow;
    }
}
