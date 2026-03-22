using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Domain.Repositories;
using FiapCloudGamesCatalog.Domain.Entities;
using Microsoft.Extensions.Logging;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Services;

public class CartServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CartService _service;

    public CartServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<CartService>();
        _service = new CartService(_uow.Object, logger);
    }

    [Fact]
    public async Task GetCartByUserAsync_NotFound_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Cart?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetCartByUserAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateCartAsync_WhenExists_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync(new Cart(Guid.NewGuid(), new List<Game>()));

        await Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => _service.CreateCartAsync(Guid.NewGuid()));
    }
}
