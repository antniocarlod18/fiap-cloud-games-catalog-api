using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Services;

public class GameServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly GameService _service;

    public GameServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<GameService>();
        _service = new GameService(_uow.Object, logger);
    }

    [Fact]
    public async Task GetAsync_NotFound_Throws()
    {
        _uow.Setup(x => x.GamesRepo.GetWithPromotionsByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Game?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetAsync(Guid.NewGuid()));
    }
}
