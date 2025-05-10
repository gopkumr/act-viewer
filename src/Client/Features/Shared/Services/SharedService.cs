using ACRViewer.BlazorServer.Core.Interface;

namespace ACRViewer.BlazorServer.Features.Navigation.Services
{
    public class SharedService(IContainerRegistryClientService containerRegistryClientService) : ISharedService
    {
        public string GetRepositoryName()
        {
            return containerRegistryClientService.GetContainerRegistryName();
        }
    }
}
