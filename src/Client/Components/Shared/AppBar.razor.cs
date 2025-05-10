using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Utilities;
using ACRViewer.BlazorServer.Features.Navigation.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using MudBlazor;
using System.Reflection.Metadata;

namespace ACRViewer.BlazorServer.Components.Shared
{
    public partial class AppBar
    {
        private string FirstName { get; set; } = "";
        private string SecondName { get; set; } = "";
        private string Email { get; set; } = "";
        private char FirstLetterOfName { get; set; }

        [Parameter]
        public MudTheme CurrentTheme { get; set; } = BlazorTheme.DefaultTheme;

        [Parameter]
        public EventCallback<MudTheme> CurrentThemeChanged { get; set; }

        [Parameter]
        public bool IsDrawOpen { get; set; } = true;

        [Parameter]
        public EventCallback<bool> IsDrawOpenChanged { get; set; }

        private string? AssemblyVersion { get; set; }

        private string? ModuleRegistryName { get; set; }


        private bool _isDarkMode = false;

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IAuthenticationManager? AuthenticationManager { get; set; }

        [Inject] private ISharedService? SharedService { get; set; }

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

                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version?.ToString() ?? "Unknown";
                AssemblyVersion = version;

                ModuleRegistryName = SharedService?.GetRepositoryName() ?? "Unknown";
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                NavigationManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", forceLoad: true);
            }
        }

        private void Logout()
        {
            NavigationManager.NavigateTo("/MicrosoftIdentity/Account/SignOut", forceLoad: true);
        }

        private async Task DarkMode()
        {
            CurrentTheme = _isDarkMode
                ? BlazorTheme.DefaultTheme
                : BlazorTheme.DarkTheme;

            _isDarkMode = !_isDarkMode;

            await CurrentThemeChanged.InvokeAsync(CurrentTheme);
        }

        private async Task ToggerDraw()
        {
            IsDrawOpen = !IsDrawOpen;
            await IsDrawOpenChanged.InvokeAsync(IsDrawOpen);
        }
    }
}
