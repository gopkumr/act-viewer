using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Core.Models;
using Arinco.BicepHub.App.Features.Navigation.Services;
using NSubstitute;

namespace Arinco.BicepHub.Tests.Features;

public class NavigationServiceTests
{
    private readonly IContainerRegistryClientService _containerRegistryClientService;
    private readonly NavigationService _navigationService;

    public NavigationServiceTests()
    {
        _containerRegistryClientService = Substitute.For<IContainerRegistryClientService>();
        _navigationService = new NavigationService(_containerRegistryClientService);
    }

    [Fact]
    public async Task GetRepositoryNames_ReturnsNames()
    {
        // Arrange
        var repositories = new List<Repository>
        {
            new Repository { Name = "repo1" },
            new Repository { Name = "repo2" }
        };
        _containerRegistryClientService.GetRepositories().Returns(Task.FromResult<IEnumerable<Repository>>(repositories));

        // Act
        var result = await _navigationService.GetRepositoryNames();

        // Assert
        Assert.Equal(new[] { "repo1", "repo2" }, result);
    }

    [Fact]
    public async Task GetRepositoryNames_EmptyList_ReturnsEmpty()
    {
        // Arrange
        _containerRegistryClientService.GetRepositories().Returns(Task.FromResult(Enumerable.Empty<Repository>()));

        // Act
        var result = await _navigationService.GetRepositoryNames();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTagNames_ReturnsNames()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new Tag { Name = "tag1", Digest = "d1" },
            new Tag { Name = "tag2", Digest = "d2" }
        };
        _containerRegistryClientService.GetTags("repo1").Returns(Task.FromResult<IEnumerable<Tag>>(tags));

        // Act
        var result = await _navigationService.GetTagNames("repo1");

        // Assert
        Assert.Equal(new[] { "tag1", "tag2" }, result);
    }

    [Fact]
    public async Task GetTagNames_EmptyList_ReturnsEmpty()
    {
        // Arrange
        _containerRegistryClientService.GetTags("repo1").Returns(Task.FromResult(Enumerable.Empty<Tag>()));

        // Act
        var result = await _navigationService.GetTagNames("repo1");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetRepositoryName_ReturnsName()
    {
        // Arrange
        _containerRegistryClientService.GetContainerRegistryName().Returns("my-registry");

        // Act
        var result = _navigationService.GetRepositoryName();

        // Assert
        Assert.Equal("my-registry", result);
    }
}
