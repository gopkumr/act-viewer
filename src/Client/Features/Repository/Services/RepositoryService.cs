using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Core.Models;

namespace Arinco.BicepHub.App.Features.Repository.Services
{
    public class RepositoryService(IContainerRegistryClientService containerRegistryClientService) : IRepositoryService
    {
        public async Task<Core.Models.Repository> GetRepository(string repositoryName)
        {
            return await containerRegistryClientService.GetRepository(repositoryName);
        }
    }
}
