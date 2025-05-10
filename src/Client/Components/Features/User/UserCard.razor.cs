using Arinco.BicepHub.App.Core.Interface;
using Microsoft.AspNetCore.Components;

namespace Arinco.BicepHub.App.Components.Features.User
{
    public partial class UserCard
    {
        [Inject] private IAuthenticationManager? AuthenticationManager { get; set; }

        [Parameter] public string Class { get; set; } = "";
        private string FirstName { get; set; } = "";
        private string SecondName { get; set; } = "";
        private string Email { get; set; } = "";
        private char FirstLetterOfName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationManager == null)
            {
                throw new InvalidOperationException("Authentication Manager not injected");
            }

            var user = await AuthenticationManager.GetAuthenticatedUser() ?? throw new InvalidOperationException("User not authenticated");
            Email = user.Email;
            FirstName = user.FirstName;
            SecondName = user.LastName;
            if (FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
        }
    }
}