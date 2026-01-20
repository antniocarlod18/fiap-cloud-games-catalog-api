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
        AddDomainEvent(new OrderCreatedDomainEvent(Id, UserId, Games, Price));
    }

    [SetsRequiredMembers]
    public Order()
    {
    }

    public void CompletedOrder()
    {
        if(Status != OrderStatusEnum.WaitingForPayment)
            throw new InvalidOrderStatusException(Status);

        Status = OrderStatusEnum.Completed;
        AddDomainEvent(new OrderCompleteDomainEvent(Id, UserId, Games));
        DateUpdated = DateTime.UtcNow;
    }

    public void CancelOrder()
    {
        if (Status != OrderStatusEnum.WaitingForPayment)
            throw new InvalidOrderStatusException(Status);

        Status = OrderStatusEnum.Canceled;
        AddDomainEvent(new OrderCanceledDomainEvent(Id, UserId, Games));
        DateUpdated = DateTime.UtcNow;
    }

    public void RefundOrder()
    {
        if(Status == OrderStatusEnum.Completed && DateTime.Now < base.DateCreated.AddDays(RefundPeriodInDays))
        {
            Status = OrderStatusEnum.Refunded;
            AddDomainEvent(new OrderRefundedDomainEvent(Id, UserId, Games));
            DateUpdated = DateTime.UtcNow;
        }
        else
        {
            throw new CannotRefundOrderException();
        }
    }
}
