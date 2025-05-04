using ACRViewer.BlazorServer.Core.Interface;

namespace ACRViewer.BlazorServer.Features.Tag.Services
{
    public class TagService(IContainerRegistryClientService containerRegistryClientService) : ITagService
    {
        public async Task<Core.Models.Tag> GetTag(string repositoryName, string tagOrDigest)
        {
            return await containerRegistryClientService.GetTag(repositoryName, tagOrDigest);
        }
    }
}
