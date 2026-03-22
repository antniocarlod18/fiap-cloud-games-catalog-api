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

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<OrderService>();
        _service = new OrderService(_uow.Object, logger);
    }

    [Fact]
    public async Task AddAsync_NoCart_Throws()
    {
        _uow.Setup(x => x.CartsRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Cart?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.AddAsync(Guid.NewGuid()));
    }
}
