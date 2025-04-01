using ACRViewer.BlazorServer.Configuration;
using ACRViewer.BlazorServer.Managers;
using ACRViewer.BlazorServer.Models;
using ACRViewer.BlazorServer.Services.Interfaces;
using Azure;
using Azure.Containers.ContainerRegistry;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ACRViewer.BlazorServer.Services
{
    public class ACRService(ACRSettings acrSettings, IAuthenticationManager authenticationManager, HttpClient httpClient) : IACRService
    {
        ContainerRegistryClient? _client;
        private User _user;

        private async Task InitialiseClient()
        {
            if (_client == null)
            {
                _user = await authenticationManager.GetAuthenticatedUser() ?? throw new InvalidOperationException("User not authenticated");
                _client = new(new Uri(acrSettings.BaseUrl), _user.AzureAccessTokenCredential);
            }
        }

        public string GetACRName()
        {
            return acrSettings.Name.Split('.').FirstOrDefault() ?? "Fully qualified name not configured";
        }

        public async Task<IEnumerable<Repository>?> GetAllRepositories()
        {
            await InitialiseClient();

            List<Repository> response = [];
            AsyncPageable<string> repositories = _client.GetRepositoryNamesAsync();

            await foreach (string repository in repositories)
            {
                response.Add(new Repository { Name = repository });
            }

            return response;
        }

        public async Task<Repository> GetRepositoryPropertiesAsync(string repository)
        {
            await InitialiseClient();
            var repositoryInstance = _client.GetRepository(repository);
            var repositoryProperties = await repositoryInstance.GetPropertiesAsync();

            return new Repository
            {
                Name = repositoryProperties.Value.Name,
                CreatedDate = repositoryProperties.Value.CreatedOn,
                ModifiedDate = repositoryProperties.Value.LastUpdatedOn,
                ManifestCount = repositoryProperties.Value.ManifestCount,
                TagCount = repositoryProperties.Value.TagCount,
                Manifest = JsonObject.Parse(repositoryProperties.GetRawResponse().Content.ToString())
            };
        }

        public async Task<Tag> GetTagPropertiesAsync(string repository, string tag)
        {
            await InitialiseClient();
            var artifact = _client.GetArtifact(repository, tag);
            var tagProperties = await artifact.GetTagPropertiesAsync(tag);
            var manifestProperties = await artifact.GetManifestPropertiesAsync();
            var response = JsonObject.Parse(manifestProperties.GetRawResponse().Content.ToString());

            return new Tag
            {
                Name = tagProperties.Value.Name,
                Digest = tagProperties.Value.Digest,
                CreationDate = tagProperties.Value.CreatedOn,
                ModifiedDate = tagProperties.Value.LastUpdatedOn,
                Platform = $"{manifestProperties.Value.OperatingSystem?.ToString() ?? "NA"}/{manifestProperties.Value.Architecture?.ToString() ?? "NA"}",
                SizeInBytes = manifestProperties.Value.SizeInBytes,
                Manifest = response
            };
        }           

        public async Task<IEnumerable<Tag>?> GetTags(string repository)
        {
            await InitialiseClient();

            List<Tag> response = [];
            var artifactManifestProperties = _client.GetRepository(repository).GetAllManifestPropertiesAsync(ArtifactManifestOrder.LastUpdatedOnDescending);
            await foreach (var property in artifactManifestProperties)
            {
                foreach (var tag in property.Tags)
                {
                    response.Add(new Tag
                    {
                        Name = tag,
                        Digest = property.Digest,
                        CreationDate = property.CreatedOn,
                        ModifiedDate = property.LastUpdatedOn,
                        Platform = $"{property.OperatingSystem?.ToString() ?? "NA"}/{property.Architecture?.ToString() ?? "NA"}",
                    });
                }
            }
            return response;
        }
    }
}
