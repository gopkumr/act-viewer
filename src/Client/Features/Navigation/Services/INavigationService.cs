﻿
namespace Arinco.BicepHub.App.Features.Navigation.Services
{
    public interface INavigationService
    {
        string GetRepositoryName();
        Task<IEnumerable<string>> GetRepositoryNames();
        Task<IEnumerable<string>> GetTagNames(string repositoryName);
    }
}