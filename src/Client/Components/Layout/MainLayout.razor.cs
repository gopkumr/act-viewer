using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using MudBlazor;

namespace ACRViewer.BlazorServer.Components.Layout
{
    public partial class MainLayout
    {
        private string CurrentUserId { get; set; } = "";
        private string ImageDataUrl { get; set; } = "";
        private string FirstName { get; set; } = "";
        private string SecondName { get; set; } = "";
        private string Email { get; set; } = "";
        private char FirstLetterOfName { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IAuthenticationManager? AuthenticationManager { get; set; }

        private MudTheme _currentTheme;
        private bool _drawerOpen = true;

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationManager == null)
            {
                throw new InvalidOperationException("Authentication Manager not injected");
            }

            try
            {
                var user = await AuthenticationManager.GetAuthenticatedUser() ?? throw new InvalidOperationException("User not authenticated");
                Email = user.Email;
                FirstName = user.FirstName;
                SecondName = user.LastName;
                if (FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }
                _currentTheme = BlazorTheme.DefaultTheme;
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavigationManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", forceLoad: true);
            }
        }

        private async Task Logout()
        {
            NavigationManager.NavigateTo("/MicrosoftIdentity/Account/SignOut", forceLoad: true);
        }

        private async Task Account()
        {
            NavigationManager.NavigateTo("https://myaccount.microsoft.com/", forceLoad: true);
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private async Task DarkMode()
        {
            //bool isDarkMode = await _clientPreferenceManager.ToggleDarkModeAsync();
            //_currentTheme = isDarkMode
            //    ? BlazorHeroTheme.DefaultTheme
            //    : BlazorHeroTheme.DarkTheme;
        }
    }
}
