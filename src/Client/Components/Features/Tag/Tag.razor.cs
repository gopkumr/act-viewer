using ACRViewer.BlazorServer.Features.Tag.Services;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Features.Tag
{
    public partial class Tag
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "tagName")]
        public string? TagName { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "repositoryName")]
        public string? RepositoryName { get; set; }

        [Inject] private ITagService? TagService { get; set; }

        private Core.Models.Tag? TagInstance { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (TagService != null && TagName != null && RepositoryName != null)
            {
                TagInstance = await TagService.GetTag(RepositoryName, TagName);
                StateHasChanged();
            }
        }
    }
}
