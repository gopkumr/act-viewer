using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Models;

namespace ACRViewer.BlazorServer.Features.Repository.Services
{
    public class RepositoryService(IContainerRegistryClientService containerRegistryClientService) : IRepositoryService
    {
        public async Task<Core.Models.Repository> GetRepository(string repositoryName)
        {
            return await containerRegistryClientService.GetRepository(repositoryName);
        }
    }
}
