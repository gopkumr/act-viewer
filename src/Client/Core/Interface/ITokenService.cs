namespace ACRViewer.BlazorServer.Core.Interface
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync(string[] scopes);
    }
}