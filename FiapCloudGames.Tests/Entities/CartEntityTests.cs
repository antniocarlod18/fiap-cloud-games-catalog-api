using System;
using System.Collections.Generic;
using Xunit;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Entities;

public class CartEntityTests
{
    [Fact]
    public void Constructor_WithEmptyGames_ShouldInitializeActiveAndAddDomainEvent()
    {
        var cart = new Cart(Guid.NewGuid(), new List<Game>());

        Assert.True(cart.Active);
        Assert.NotNull(cart.DomainEvents);
        Assert.Contains(cart.DomainEvents, e => e.GetType().Name.Contains("CartCreatedDomainEvent"));
    }

    [Fact]
    public void AddGame_Duplicate_Throws()
    {
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        var cart = new Cart(Guid.NewGuid(), new List<Game>{ game });

        Assert.Throws<InvalidOperationAddingGameToCartException>(() => cart.AddGame(game));
    }
}
