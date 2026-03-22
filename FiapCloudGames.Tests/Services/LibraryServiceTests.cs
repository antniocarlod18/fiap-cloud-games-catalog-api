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

public class LibraryServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly LibraryService _service;

    public LibraryServiceTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<LibraryService>();
        _service = new LibraryService(_uow.Object, logger);
    }

    [Fact]
    public async Task GetLibraryByUserAsync_NotFound_Throws()
    {
        _uow.Setup(x => x.LibrariesRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((FiapCloudGamesCatalog.Domain.Entities.Library?)null);

        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetLibraryByUserAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateLibraryAsync_WhenExists_Throws()
    {
        _uow.Setup(x => x.LibrariesRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync(new FiapCloudGamesCatalog.Domain.Entities.Library(Guid.NewGuid(), new List<Game>()));

        await Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => _service.CreateLibraryAsync(Guid.NewGuid()));
    }
}
