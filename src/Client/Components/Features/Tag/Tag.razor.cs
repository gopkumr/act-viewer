using ACRViewer.BlazorServer.Features.Tag.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

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

        [Inject] private IJSRuntime JSRuntime { get; set; }

        private Core.Models.Tag? TagInstance { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (TagService != null && TagName != null && RepositoryName != null)
            {
                TagInstance = await TagService.GetTag(RepositoryName, TagName);
                StateHasChanged();
            }
        }
        private async Task DownloadSource()
        {
            if (TagService != null && TagName != null && RepositoryName != null)
            {
                var source = await TagService.DownloadTagSourceCode(RepositoryName, TagName);
                if (source != null)
                {
                    var fileName=$"{RepositoryName.Split('/').Last()}.{TagName}.bicep";
                    var base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
                    await JSRuntime.InvokeVoidAsync("downloadFile", fileName, base64Content, "text/plain");
                }
            }
        }
    }
}
