using System;
using System.Collections.Generic;
using Xunit;
using FiapCloudGamesCatalog.Domain.Entities;

namespace FiapCloudGames.Tests.Entities;

public class GameEntityTests
{
    [Fact]
    public void UpdatePrice_AddsDomainEventAndUpdatesPrice()
    {
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        game.UpdatePrice(5m);

        Assert.Equal(5m, game.Price);
        Assert.Contains(game.DomainEvents, e => e.GetType().Name.Contains("GamePriceUpdatedDomainEvent"));
    }

    [Fact]
    public void GetPriceWithDiscount_NoPromotions_ReturnsPrice()
    {
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        var price = game.GetPriceWithDiscount();
        Assert.Equal(10m, price);
    }
}
