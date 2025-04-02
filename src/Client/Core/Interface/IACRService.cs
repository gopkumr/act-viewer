using ACRViewer.BlazorServer.Core.Models;

namespace ACRViewer.BlazorServer.Core.Interface
{
    public interface IACRService
    {
        Task<IEnumerable<Repository>?> GetAllRepositories();
        string GetACRName();

        Task<IEnumerable<Tag>?> GetTags(string repository);

        Task<Repository> GetRepositoryPropertiesAsync(string repository);

        Task<Tag> GetTagPropertiesAsync(string repository, string tag);
    }
}