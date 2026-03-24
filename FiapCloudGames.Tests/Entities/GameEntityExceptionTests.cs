using System;
using System.Collections.Generic;
using Xunit;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Entities;

public class GameEntityExceptionTests
{
    [Fact]
    public void UpdatePrice_TriggersEvent()
    {
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        game.UpdatePrice(12m);
        Assert.Equal(12m, game.Price);
        Assert.Contains(game.DomainEvents, e => e.GetType().Name.Contains("GamePriceUpdatedDomainEvent"));
    }
}
