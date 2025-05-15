using Arinco.BicepHub.App.Core.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;

namespace Arinco.BicepHub.App.Components.Shared
{
    public partial class Landing
    {
        [Inject] AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        [Inject] IAuthenticationManager? AuthenticationManager { get; set; }

        [Inject] NavigationManager? NavigationManager { get; set; }

        [Inject] MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateProvider == null)
                throw new InvalidOperationException("AuthenticationStateProvider not injected");
            if (AuthenticationManager == null)
                throw new InvalidOperationException("AuthenticationManager not injected");
            if (NavigationManager == null)
                throw new InvalidOperationException("NavigationManager not injected");

            var returnUrl = NavigationManager.Uri.Contains("returnUrl")
                ? NavigationManager.GetUriWithQueryParameter("returnUrl", (string?)null)
                : "/home";

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!(authState?.User?.Identity?.IsAuthenticated ?? false))
                NavigationManager.NavigateTo("/login?returnUrl=" + Uri.EscapeDataString(returnUrl), true);

            try
            {
                var user = await AuthenticationManager.GetAuthenticatedUser() ?? throw new InvalidOperationException("User not authenticated");
                if (user.AzureAccessTokenCredential == null)
                    throw new InvalidOperationException("User not authenticated");
            }
            catch (MicrosoftIdentityWebChallengeUserException ex)
            {
                ConsentHandler.HandleException(ex);
                return;
            }
            catch (Exception)
            {
                NavigationManager.NavigateTo("/login?returnUrl=" + Uri.EscapeDataString(returnUrl), true);
                return;
            }

            NavigationManager.NavigateTo(returnUrl, forceLoad: true);
            await base.OnInitializedAsync();
        }
    }
}
