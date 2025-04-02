using ACRViewer.BlazorServer.Core.Interface;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Features.Repository
{
    public partial class Repository
    {
        [Parameter]
        public string? RepositoryName { get; set; }

        [Inject] private IACRService? ACRService { get; set; }

        private Core.Models.Repository? RepositoryInstance { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (ACRService != null && RepositoryName != null)
            {
                RepositoryInstance = await ACRService.GetRepositoryPropertiesAsync(RepositoryName);
                StateHasChanged();
            }

            await base.OnParametersSetAsync();
        }
    }
}
