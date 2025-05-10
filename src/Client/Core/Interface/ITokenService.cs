namespace Arinco.BicepHub.App.Core.Interface
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync(string[] scopes);
    }
}