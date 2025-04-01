using ACRViewer.BlazorServer.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Shared
{
    public partial class Tag
    {
        [Parameter]
        public string? TagName { get; set; }

        [Parameter]
        public string? RepositoryName { get; set; }

        [Inject] private IACRService? ACRService { get; set; }

        private ACRViewer.BlazorServer.Models.Tag? TagInstance{ get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (ACRService != null && TagName != null && RepositoryName!=null)
            {
                TagInstance = await ACRService.GetTagPropertiesAsync(RepositoryName, TagName);
                StateHasChanged();
            }
        }
    }
}
