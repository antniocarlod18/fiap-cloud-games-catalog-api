using System;
using System.Collections.Generic;
using Xunit;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Entities;

public class PromotionEntityTests
{
    [Fact]
    public void AddGame_Duplicate_Throws()
    {
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        var promotion = new Promotion(new List<Game>{ game }, 10m, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
        Assert.Throws<InvalidOperationAddingGameToCartException>(() => promotion.AddGame(game));
    }

    [Fact]
    public void Active_Property_BasedOnDates()
    {
        var promotion = new Promotion(new List<Game>(), 10m, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
        Assert.True(promotion.Active);
    }
}
