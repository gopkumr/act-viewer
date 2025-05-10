using Azure.Core;

namespace Arinco.BicepHub.App.Core.Utilities;

public class CustomTokenCredential(string token) : TokenCredential
{
    private readonly string _token = token;

    public string Token => _token;

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new AccessToken(_token, DateTimeOffset.UtcNow.AddHours(1)); // Set expiration (e.g., 1 hour)
    }

    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new ValueTask<AccessToken>(GetToken(requestContext, cancellationToken));
    }
}
