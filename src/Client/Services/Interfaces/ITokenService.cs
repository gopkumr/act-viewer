namespace ACRViewer.BlazorServer.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync(string[] scopes);
    }
}