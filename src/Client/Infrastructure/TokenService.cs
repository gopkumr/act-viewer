using Arinco.BicepHub.App.Core.Interface;
using Microsoft.Identity.Web;

namespace Arinco.BicepHub.App.Infrastructure
{
    public class TokenService(
         ITokenAcquisition tokenAcquisition,
         MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler,
         ILogger<TokenService> logger
        ) : ITokenService
    {
        public async Task<string> GetAccessTokenAsync(string[] scopes)
        {
            try
            {
                // Attempt to get token silently
                return await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            }
            catch (MicrosoftIdentityWebChallengeUserException ex)
            {
                logger.LogWarning(ex, "Silent token acquisition failed. Handling consent and retry.");
                throw;
            }
            catch (Exception ex1)
            {
                logger.LogError(ex1, "An error occurred while acquiring the access token.");
                return string.Empty;
            }
        }
    }
}
