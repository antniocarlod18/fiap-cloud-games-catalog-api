using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Services;

public class CartServiceExceptionTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CartService _service;

    public CartServiceExceptionTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<CartService>();
        _service = new CartService(_uow.Object, logger);
    }

    [Fact]
    public async Task AddGameToCart_CartNotFound_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Cart?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.AddGameToCart(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task AddGameToCart_GameNotFound_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync(new Cart(Guid.NewGuid(), new List<Game>()));
        _uow.Setup(x => x.GamesRepo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Game?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.AddGameToCart(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task AddGamesToCart_GamesNotFound_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync(new Cart(Guid.NewGuid(), new List<Game>()));
        _uow.Setup(x => x.GamesRepo.GetByIds(It.IsAny<IList<Guid>>())).ReturnsAsync((IList<Game>?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.AddGamesToCart(Guid.NewGuid(), new List<Guid>{ Guid.NewGuid() }));
    }

    [Fact]
    public async Task RemoveGameFromCart_CartNotFound_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Cart?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.RemoveGameFromCart(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task RemoveGamesFromCart_CartNotFound_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Cart?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.RemoveGamesFromCart(Guid.NewGuid(), new List<Guid>{ Guid.NewGuid() }));
    }
}
