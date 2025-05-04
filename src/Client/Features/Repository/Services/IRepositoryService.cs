





namespace ACRViewer.BlazorServer.Features.Repository.Services
{
    public interface IRepositoryService
    {
        Task<Core.Models.Repository> GetRepository(string repositoryName);
    }
}