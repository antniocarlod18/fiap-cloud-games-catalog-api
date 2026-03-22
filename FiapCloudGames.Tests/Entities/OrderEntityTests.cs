using System;
using System.Collections.Generic;
using Xunit;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;
using FiapCloudGamesCatalog.Domain.Enums;

namespace FiapCloudGames.Tests.Entities;

public class OrderEntityTests
{
    [Fact]
    public void Constructor_EmptyGames_Throws()
    {
        Assert.Throws<CannotCreateAnOrderWithoutItemsException>(() => new Order(Guid.NewGuid(), new List<Game>()));
    }

    [Fact]
    public void CompletedOrder_TransitionsStatusAndAddsEvent()
    {
        var g = new Game("T", "G", new List<GamePlatformEnum>{GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        var order = new Order(Guid.NewGuid(), new List<Game>{ g });
        order.CompletedOrder();
        Assert.Equal(OrderStatusEnum.Completed, order.Status);
        Assert.Contains(order.DomainEvents, e => e.GetType().Name.Contains("OrderCompleteDomainEvent"));
    }
}
