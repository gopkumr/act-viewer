using Microsoft.AspNetCore.Components;

namespace Arinco.BicepHub.App.Components.Features.User
{
    public partial class LoginDisplay
    {
        private bool isAuthenticated;
        private string userName = string.Empty;
        private string reason = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            isAuthenticated = authState.User.Identity?.IsAuthenticated ?? false;
            userName = authState.User.Identity?.Name ?? "Unknown";

            if(Navigation.Uri.Contains("reason"))
            {
                var reason = Navigation.GetUriWithQueryParameter("reason", (string?)null);
                if (reason == "access-denied")
                {
                    reason = "Access Denied: You do not have required access, please contact admin";
                }
            }

        }

        private void SignIn()
        {
            var returnUrl = Navigation.Uri.Contains("returnUrl")
                ? Navigation.GetUriWithQueryParameter("returnUrl", (string?)null)
                : "/";
            Navigation.NavigateTo($"MicrosoftIdentity/Account/SignIn?returnUrl={Uri.EscapeDataString(returnUrl)}", true);
        }

        private void Logout()
        {
            Navigation.NavigateTo("/MicrosoftIdentity/Account/SignOut", true);
        }
    }
}
