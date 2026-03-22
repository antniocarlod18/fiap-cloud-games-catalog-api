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

public class PromotionServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly PromotionService _service;

    public PromotionServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PromotionService>();
        _service = new PromotionService(_uow.Object, logger);
    }

    [Fact]
    public async Task GetAsync_NotFound_Throws()
    {
        _uow.Setup(x => x.PromotionsRepo.GetDetailedByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Promotion?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetAsync(Guid.NewGuid()));
    }
}
