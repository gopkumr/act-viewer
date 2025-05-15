using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Core.Models;
using Arinco.BicepHub.App.Features.Repository.Services;
using NSubstitute;


namespace Arinco.BicepHub.Tests.Features;

public class RepositoryServiceTests
{
    private readonly IContainerRegistryClientService _containerRegistryClientService;
    private readonly RepositoryService _repositoryService;

    public RepositoryServiceTests()
    {
        _containerRegistryClientService = Substitute.For<IContainerRegistryClientService>();
        _repositoryService = new RepositoryService(_containerRegistryClientService);
    }

    [Fact]
    public async Task GetRepository_ReturnsRepository()
    {
        // Arrange
        var expected = new Repository
        {
            Name = "repo1",
            RegistryLoginServer = "server",
            CreatedDate = DateTimeOffset.UtcNow,
            ModifiedDate = DateTimeOffset.UtcNow,
            ManifestCount = 1,
            TagCount = 2,
            Manifest = null
        };
        _containerRegistryClientService.GetRepository("repo1")
            .Returns(Task.FromResult(expected));

        // Act
        var result = await _repositoryService.GetRepository("repo1");

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetRepository_WhenNullReturned_ReturnsNull()
    {
        // Arrange
        _containerRegistryClientService.GetRepository("repo2")
            .Returns(Task.FromResult<Repository>(null));

        // Act
        var result = await _repositoryService.GetRepository("repo2");

        // Assert
        Assert.Null(result);
    }
}
