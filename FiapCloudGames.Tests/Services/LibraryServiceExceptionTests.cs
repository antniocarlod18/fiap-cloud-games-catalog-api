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

public class LibraryServiceExceptionTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly LibraryService _service;

    public LibraryServiceExceptionTests()
    {
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<LibraryService>();
        _service = new LibraryService(_uow.Object, logger);
    }

    [Fact]
    public async Task AddGamesToLibrary_LibraryNotFound_Throws()
    {
        _uow.Setup(x => x.LibrariesRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Library?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.AddGamesToLibrary(Guid.NewGuid(), new List<Guid>{ Guid.NewGuid() }));
    }

    [Fact]
    public async Task AddGamesToLibrary_GamesNotFound_Throws()
    {
        _uow.Setup(x => x.LibrariesRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), new List<Game>()));
        _uow.Setup(x => x.GamesRepo.GetByIds(It.IsAny<IList<Guid>>())).ReturnsAsync((IList<Game>?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.AddGamesToLibrary(Guid.NewGuid(), new List<Guid>{ Guid.NewGuid() }));
    }

    [Fact]
    public async Task RemoveGamesFromLibrary_LibraryNotFound_Throws()
    {
        _uow.Setup(x => x.LibrariesRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync((Library?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.RemoveGamesFromLibrary(Guid.NewGuid(), new List<Guid>{ Guid.NewGuid() }));
    }

    [Fact]
    public async Task RemoveGamesFromLibrary_GamesNotFound_Throws()
    {
        _uow.Setup(x => x.LibrariesRepo.GetByUserAsync(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), new List<Game>()));
        _uow.Setup(x => x.GamesRepo.GetByIds(It.IsAny<IList<Guid>>())).ReturnsAsync((IList<Game>?)null);
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.RemoveGamesFromLibrary(Guid.NewGuid(), new List<Guid>{ Guid.NewGuid() }));
    }
}
