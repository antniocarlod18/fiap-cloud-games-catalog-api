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

public class AuditServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AuditService _service;

    public AuditServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<AuditService>();
        _service = new AuditService(_uow.Object, logger);
    }

    [Fact]
    public async Task GetByGameAsync_GameNotFound_Throws()
    {
        _uow.Setup(x => x.GamesRepo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Game?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetByGameAsync(Guid.NewGuid(), null));
    }
}
