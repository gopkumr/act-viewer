﻿using ACRViewer.BlazorServer.Services.Interfaces;
using Microsoft.Identity.Web;

namespace ACRViewer.BlazorServer.Services
{
    public class TokenService(
         IHttpContextAccessor httpContextAccessor,
         ITokenAcquisition tokenAcquisition

        ) : ITokenService
    {
       public async Task<string> GetAccessTokenAsync(string[] scopes)
        {
            try
            {
                // Attempt to get token silently
                return await tokenAcquisition.GetAccessTokenForUserAsync(scopes, user: httpContextAccessor.HttpContext?.User);
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                // Silent acquisition failed (e.g., token expired), trigger interactive login
                throw; // Let the Blazor app handle redirection to sign-in
            }
        }
    }
}
