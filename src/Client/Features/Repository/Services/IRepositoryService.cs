





namespace Arinco.BicepHub.App.Features.Repository.Services
{
    public interface IRepositoryService
    {
        Task<Core.Models.Repository> GetRepository(string repositoryName);
    }
}