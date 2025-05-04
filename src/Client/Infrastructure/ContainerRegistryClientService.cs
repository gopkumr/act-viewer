using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Models;
using ACRViewer.BlazorServer.Core.Utilities;
using Azure;
using Azure.Containers.ContainerRegistry;
using System.Text.Json.Nodes;

namespace ACRViewer.BlazorServer.Infrastructure
{
    public class ContainerRegistryClientService(ACRSettings acrSettings, IAuthenticationManager authenticationManager) : IContainerRegistryClientService
    {
        private ContainerRegistryClient? _client;
        private User? _user;

        private async Task InitialiseClient()
        {
            if (_client == null)
            {
                _user = await authenticationManager.GetAuthenticatedUser() ?? throw new InvalidOperationException("User not authenticated");
                _client = new(new Uri(acrSettings.BaseUrl), _user.AzureAccessTokenCredential);
            }
        }

        public string GetContainerRegistryName()
        {
            return acrSettings.Name.Split('.').FirstOrDefault() ?? "Fully qualified name not configured";
        }


        public async Task<IEnumerable<Repository>> GetRepositories()
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

        public async Task<Repository> GetRepository(string repositoryName)
        {
            await InitialiseClient();
            var repositoryInstance = _client.GetRepository(repositoryName);
            var repositoryProperties = await repositoryInstance.GetPropertiesAsync();

            return new Repository
            {
                Name = repositoryProperties.Value.Name,
                CreatedDate = repositoryProperties.Value.CreatedOn,
                ModifiedDate = repositoryProperties.Value.LastUpdatedOn,
                ManifestCount = repositoryProperties.Value.ManifestCount,
                TagCount = repositoryProperties.Value.TagCount,
                Manifest = JsonNode.Parse(repositoryProperties.GetRawResponse().Content.ToString())
            };
        }

        public async Task<Tag> GetTag(string repositoryName, string tagNameOfDigest)
        {
            await InitialiseClient();
            var artifact = _client.GetArtifact(repositoryName, tagNameOfDigest);
            var tagProperties = await artifact.GetTagPropertiesAsync(tagNameOfDigest);
            var manifestProperties = await artifact.GetManifestPropertiesAsync();
            var response = JsonNode.Parse(manifestProperties.GetRawResponse().Content.ToString());

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

        public async Task<IEnumerable<Tag>> GetTags(string repositoryName)
        {
            await InitialiseClient();

            List<Tag> response = [];
            var artifactManifestProperties = _client.GetRepository(repositoryName).GetAllManifestPropertiesAsync(ArtifactManifestOrder.LastUpdatedOnDescending);
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
