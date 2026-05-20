using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FiapCloudGamesCatalog.Application.Services;
using FiapCloudGamesCatalog.Application.Services.Interfaces;
using FiapCloudGamesCatalog.Domain.Repositories;
using Microsoft.Extensions.Logging;
using FiapCloudGamesCatalog.Domain.Entities;
using FiapCloudGamesCatalog.Application.Dtos;
using FiapCloudGamesCatalog.Domain.Exceptions;

namespace FiapCloudGames.Tests.Services;

public class GameServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IGameIndexer> _indexer = new();
    private readonly Mock<ICacheService> _cache = new();
    private readonly GameService _service;

    public GameServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<GameService>();
        _indexer.Setup(x => x.IndexAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _cache.Setup(x => x.GetAsync<GameResponseDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GameResponseDto?)null);
        _cache.Setup(x => x.GetAsync<List<GameResponseDto?>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<GameResponseDto?>?)null);
        _cache.Setup(x => x.RemoveByPrefixAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _service = new GameService(_uow.Object, _indexer.Object, _cache.Object, logger);
    }

    [Fact]
    public async Task GetAsync_NotFound_Throws()
    {
        _uow.Setup(x => x.GamesRepo.GetWithPromotionsByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Game?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetAsync(Guid.NewGuid()));
    }
}
