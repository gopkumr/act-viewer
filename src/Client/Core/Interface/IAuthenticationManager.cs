using Arinco.BicepHub.App.Core.Models;

namespace Arinco.BicepHub.App.Core.Interface
{
    public interface IAuthenticationManager
    {
        Task<User?> GetAuthenticatedUser();
    }
}