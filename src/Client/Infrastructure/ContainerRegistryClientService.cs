using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Models;
using ACRViewer.BlazorServer.Core.Utilities;
using Azure;
using Azure.Containers.ContainerRegistry;
using SharpCompress.Archives.Tar;
using SharpCompress.Compressors.Deflate;
using System;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;

namespace ACRViewer.BlazorServer.Infrastructure
{
    public class ContainerRegistryClientService(ACRSettings acrSettings, IAuthenticationManager authenticationManager, HttpClient httpClient)
        : IContainerRegistryClientService
    {
        private ContainerRegistryClient? _client;
        private ContainerRegistryContentClient? _contentClient;
        private User? _user;

        private async Task InitialiseClient(string? repositoryName = null)
        {
            _user ??= await authenticationManager.GetAuthenticatedUser() ?? throw new InvalidOperationException("User not authenticated");
            _client ??= new(new Uri(acrSettings.BaseUrl), _user.AzureAccessTokenCredential);

            if (repositoryName != null && (_contentClient == null || !_contentClient.RepositoryName.Equals(repositoryName, StringComparison.InvariantCultureIgnoreCase)))
            {
                _contentClient = new(new Uri(acrSettings.BaseUrl), repositoryName, _user.AzureAccessTokenCredential);
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
            AsyncPageable<string> repositories = _client!.GetRepositoryNamesAsync();

            await foreach (string repository in repositories)
            {
                response.Add(new Repository { Name = repository });
            }

            return response;
        }

        public async Task<Repository> GetRepository(string repositoryName)
        {
            await InitialiseClient();
            var repositoryInstance = _client!.GetRepository(repositoryName);
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
            await InitialiseClient(repositoryName);
            var artifact = _client!.GetArtifact(repositoryName, tagNameOfDigest);
            var tagProperties = await artifact.GetTagPropertiesAsync(tagNameOfDigest);
            var manifestProperties = await artifact.GetManifestPropertiesAsync();
            var manifestValue = await _contentClient!.GetManifestAsync(tagNameOfDigest);
            var response = JsonNode.Parse(manifestValue.Value.Manifest.ToString());

            return new Tag
            {
                Name = tagProperties.Value.Name,
                Digest = tagProperties.Value.Digest,
                CreationDate = tagProperties.Value.CreatedOn,
                ModifiedDate = tagProperties.Value.LastUpdatedOn,
                Platform = $"{manifestProperties.Value.OperatingSystem?.ToString() ?? "NA"}/{manifestProperties.Value.Architecture?.ToString() ?? "NA"}",
                SizeInBytes = manifestProperties.Value.SizeInBytes,
                Manifest = response,
                Source = await GetTagSource(repositoryName, tagNameOfDigest),
                Documentation = await GetTagDocumentation(repositoryName, tagNameOfDigest)
            };
        }

        public async Task<IEnumerable<Tag>> GetTags(string repositoryName)
        {
            await InitialiseClient();

            List<Tag> response = [];
            var artifactManifestProperties = _client!.GetRepository(repositoryName).GetAllManifestPropertiesAsync(ArtifactManifestOrder.LastUpdatedOnDescending);
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

        public async Task<string> GetTagSource(string repositoryName, string tagNameOfDigest)
        {
            string? sourceFile = null;
            await InitialiseClient(repositoryName);
            var artifact = _client!.GetArtifact(repositoryName, tagNameOfDigest);
            GetManifestResult result = await _contentClient!.GetManifestAsync(tagNameOfDigest);
            OciImageManifest? manifest = result?.Manifest?.ToObjectFromJson<OciImageManifest>();
            if (manifest != null)
            {
                var sourceLayers = manifest.Layers.Where(q => q.Annotations != null && q.Annotations.Title.Equals("source files", StringComparison.OrdinalIgnoreCase))
                                                  .ToList();
                foreach (OciDescriptor layerInfo in sourceLayers)
                {
                    using var layerStream = new MemoryStream();
                    await _contentClient.DownloadBlobToAsync(layerInfo.Digest, layerStream);
                    layerStream.Position = 0;
                    using var gzipStream = new System.IO.Compression.GZipStream(layerStream, CompressionMode.Decompress);
                    using var decompressedStream = new MemoryStream();
                    await gzipStream.CopyToAsync(decompressedStream);
                    decompressedStream.Position = 0;
                    using var tarArchive = TarArchive.Open(decompressedStream);
                    foreach (var entry in tarArchive.Entries)
                    {
                        if (Path.GetExtension(entry.Key ?? "").Equals(".bicep", StringComparison.OrdinalIgnoreCase))
                        {
                            using var entryStream = entry.OpenEntryStream();
                            using var entryMemoryStream = new MemoryStream();
                            await entryStream.CopyToAsync(entryMemoryStream);
                            entryMemoryStream.Position = 0;
                            using var reader = new StreamReader(entryMemoryStream, Encoding.UTF8);
                            sourceFile = (sourceFile ?? "") + await reader.ReadToEndAsync();
                        }
                    }
                }
            }
            return sourceFile ?? "No source attached with the module";
        }

        private async Task<string> GetTagDocumentation(string repositoryName, string tagNameOfDigest)
        {
            await InitialiseClient(repositoryName);
            GetManifestResult result = await _contentClient!.GetManifestAsync(tagNameOfDigest);
            OciImageManifest? manifest = result?.Manifest?.ToObjectFromJson<OciImageManifest>();
            if (manifest != null && manifest.Annotations!=null && manifest.Annotations.Documentation!=null)
            {
                var response = await httpClient.GetAsync(manifest.Annotations.Documentation, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return "No accessible documentaion provided";
        }
    }
}
