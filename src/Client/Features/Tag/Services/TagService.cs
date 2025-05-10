using Arinco.BicepHub.App.Core.Interface;

namespace Arinco.BicepHub.App.Features.Tag.Services
{
    public class TagService(IContainerRegistryClientService containerRegistryClientService) : ITagService
    {
        public async Task<Core.Models.Tag> GetTag(string repositoryName, string tagOrDigest)
        {
            return await containerRegistryClientService.GetTag(repositoryName, tagOrDigest);
        }

        public async Task<string> DownloadTagSourceCode(string repositoryName, string tagOrDigest)
        {
            return await containerRegistryClientService.GetTagSource(repositoryName, tagOrDigest);
        }
    }
}
