





namespace ACRViewer.BlazorServer.Features.Tag.Services
{
    public interface ITagService
    {
        Task<Core.Models.Tag> GetTag(string repositoryName, string tagOrDigest);
        Task<string> DownloadTagSourceCode(string repositoryName, string tagOrDigest);
    }
}