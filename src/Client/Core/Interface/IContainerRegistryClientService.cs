using Arinco.BicepHub.App.Core.Models;

namespace Arinco.BicepHub.App.Core.Interface
{
    public interface IContainerRegistryClientService
    {
        public string GetContainerRegistryName();
        public Task<IEnumerable<Repository>> GetRepositories();

        public Task<Repository> GetRepository(string repositoryName);

        public Task<IEnumerable<Tag>> GetTags(string repositoryName);

        public Task<Tag> GetTag(string repositoryName, string tagNameOfDigest);

        Task<string> GetTagSource(string repositoryName, string tagNameOfDigest);
    }
}
