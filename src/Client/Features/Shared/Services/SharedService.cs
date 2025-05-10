using Arinco.BicepHub.App.Core.Interface;

namespace Arinco.BicepHub.App.Features.Navigation.Services
{
    public class SharedService(IContainerRegistryClientService containerRegistryClientService) : ISharedService
    {
        public string GetRepositoryName()
        {
            return containerRegistryClientService.GetContainerRegistryName();
        }
    }
}
