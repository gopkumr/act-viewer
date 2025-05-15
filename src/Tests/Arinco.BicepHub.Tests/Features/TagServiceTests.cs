using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Features.Tag.Services;
using NSubstitute;

namespace Arinco.BicepHub.Tests.Features;

public class TagServiceTests
{
    private readonly IContainerRegistryClientService _containerRegistryClientService;
    private readonly TagService _tagService;

    public TagServiceTests()
    {
        _containerRegistryClientService = Substitute.For<IContainerRegistryClientService>();
        _tagService = new TagService(_containerRegistryClientService);
    }

    [Fact]
    public async Task GetTag_ReturnsTag()
    {
        // Arrange
        var expected = new App.Core.Models.Tag
        {
            Name = "tag1",
            Digest = "digest1",
            CreationDate = DateTimeOffset.UtcNow,
            ModifiedDate = DateTimeOffset.UtcNow
        };
        _containerRegistryClientService.GetTag("repo", "tag1")
            .Returns(Task.FromResult(expected));

        // Act
        var result = await _tagService.GetTag("repo", "tag1");

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetTag_WhenNullReturned_ReturnsNull()
    {
        // Arrange
        _containerRegistryClientService.GetTag("repo", "tag2")
            .Returns(Task.FromResult<App.Core.Models.Tag>(null));

        // Act
        var result = await _tagService.GetTag("repo", "tag2");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTag_WhenExceptionThrown_PropagatesException()
    {
        // Arrange
        _containerRegistryClientService.GetTag("repo", "tag3")
            .Returns<Task<App.Core.Models.Tag>>(x => throw new InvalidOperationException("error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _tagService.GetTag("repo", "tag3"));
    }

    [Fact]
    public async Task DownloadTagSourceCode_ReturnsSource()
    {
        // Arrange
        _containerRegistryClientService.GetTagSource("repo", "tag1")
            .Returns(Task.FromResult("source-code"));

        // Act
        var result = await _tagService.DownloadTagSourceCode("repo", "tag1");

        // Assert
        Assert.Equal("source-code", result);
    }

    [Fact]
    public async Task DownloadTagSourceCode_WhenNullReturned_ReturnsNull()
    {
        // Arrange
        _containerRegistryClientService.GetTagSource("repo", "tag2")
            .Returns(Task.FromResult<string>(null));

        // Act
        var result = await _tagService.DownloadTagSourceCode("repo", "tag2");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DownloadTagSourceCode_WhenExceptionThrown_PropagatesException()
    {
        // Arrange
        _containerRegistryClientService.GetTagSource("repo", "tag3")
            .Returns<Task<string>>(x => throw new InvalidOperationException("error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _tagService.DownloadTagSourceCode("repo", "tag3"));
    }
}
