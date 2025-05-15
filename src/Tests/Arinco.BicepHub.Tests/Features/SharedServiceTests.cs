using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Features.Shared.Services;
using NSubstitute;


namespace Arinco.BicepHub.Tests.Features;

public class SharedServiceTests
{
    private readonly IContainerRegistryClientService _containerRegistryClientService;
    private readonly SharedService _sharedService;

    public SharedServiceTests()
    {
        _containerRegistryClientService = Substitute.For<IContainerRegistryClientService>();
        _sharedService = new SharedService(_containerRegistryClientService);
    }

    [Fact]
    public void GetRepositoryName_ReturnsExpectedName()
    {
        // Arrange
        _containerRegistryClientService.GetContainerRegistryName().Returns("test-repo");

        // Act
        var result = _sharedService.GetRepositoryName();

        // Assert
        Assert.Equal("test-repo", result);
    }

    [Fact]
    public void GetRepositoryName_WhenNull_ReturnsNull()
    {
        // Arrange
        _containerRegistryClientService.GetContainerRegistryName().Returns((string?)null);

        // Act
        var result = _sharedService.GetRepositoryName();

        // Assert
        Assert.Null(result);
    }
}
