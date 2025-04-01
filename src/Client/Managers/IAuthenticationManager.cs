using ACRViewer.BlazorServer.Models;

namespace ACRViewer.BlazorServer.Managers
{
    public interface IAuthenticationManager
    {
        Task<User?> GetAuthenticatedUser();
    }
}