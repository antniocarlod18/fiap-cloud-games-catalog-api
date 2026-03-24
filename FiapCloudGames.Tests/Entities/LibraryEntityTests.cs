using System;
using System.Collections.Generic;
using Xunit;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Entities;

public class LibraryEntityTests
{
    [Fact]
    public void Add_Duplicate_Throws()
    {
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        var library = new Library(Guid.NewGuid(), new List<Game>{ game });
        Assert.Throws<InvalidOperationAddingGameToCartException>(() => library.Add(game));
    }

    [Fact]
    public void Remove_NotExisting_NoThrow()
    {
        var library = new Library(Guid.NewGuid(), new List<Game>());
        var game = new Game("T", "G", new List<FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum>{FiapCloudGamesCatalog.Domain.Enums.GamePlatformEnum.PC}, "D", 10m, "Dev", "Dist", "1.0", true);
        library.Remove(game); // should not throw
        Assert.False(library.Games.Contains(game));
    }
}
