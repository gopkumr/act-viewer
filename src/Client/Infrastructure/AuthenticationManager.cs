using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Core.Models;
using Arinco.BicepHub.App.Core.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace Arinco.BicepHub.App.Infrastructure
{
    public class AuthenticationManager(
            AuthenticationStateProvider authenticationStateProvider,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache) : IAuthenticationManager
    {
        public async Task<User?> GetAuthenticatedUser()
        {
            if (memoryCache.TryGetValue("AuthenticatedUser", out User? authenticatedUser))
            {
                return authenticatedUser;
            }

            if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != true)
            {
                throw new MicrosoftIdentityWebChallengeUserException(new MsalUiRequiredException("401", "User is not authenticated. Please sign in first."), []);
            }

            var accessToken = await tokenService.GetAccessTokenAsync(scopes: ["https://management.azure.com/.default"]);
            var state = await authenticationStateProvider.GetAuthenticationStateAsync() ?? throw new MicrosoftIdentityWebChallengeUserException(new MsalUiRequiredException("401", "User is not authenticated. Please sign in first."), []);

            var email = state.User.Claims?.FirstOrDefault(q => q.Type.Equals("preferred_username", StringComparison.InvariantCultureIgnoreCase))?.Value ?? "No Email";
            var name = state.User.Claims?.FirstOrDefault(q => q.Type.Equals("name", StringComparison.InvariantCultureIgnoreCase))?.Value ?? "No Name";

            authenticatedUser = new User
            {
                Name = name,
                Email = email,
                FirstName = name.Split(' ').First(),
                LastName = name.Split(' ').Last(),
                AzureAccessTokenCredential = new CustomTokenCredential(accessToken)
            };
            memoryCache.Set("AuthenticatedUser", authenticatedUser, TimeSpan.FromMinutes(5));

            return authenticatedUser;
        }

        public void Logout()
        {
            if (memoryCache.TryGetValue("AuthenticatedUser", out User? _))
            {
                memoryCache.Remove("AuthenticatedUser");
            }

        }
    }
}
