using ACRViewer.BlazorServer.Core.Interface;

namespace ACRViewer.BlazorServer.Features.Navigation.Services
{
    public class NavigationService(IContainerRegistryClientService containerRegistryClientService) : INavigationService
    {
        public async Task<IEnumerable<string>> GetRepositoryNames()
        {
            var repositories = await containerRegistryClientService.GetRepositories();
            return repositories.Select(q => q.Name).ToList();
        }

        public async Task<IEnumerable<string>> GetTagNames(string repositoryName)
        {
            var tags = await containerRegistryClientService.GetTags(repositoryName);
            return tags.Select(q => q.Name).ToList();
        }

        public string GetRepositoryName()
        {
            return containerRegistryClientService.GetContainerRegistryName();
        }
    }
}
