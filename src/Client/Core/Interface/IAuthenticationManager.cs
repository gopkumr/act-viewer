using ACRViewer.BlazorServer.Core.Models;

namespace ACRViewer.BlazorServer.Core.Interface
{
    public interface IAuthenticationManager
    {
        Task<User?> GetAuthenticatedUser();
    }
}