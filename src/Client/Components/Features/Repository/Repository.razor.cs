using ACRViewer.BlazorServer.Features.Repository.Services;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Features.Repository
{
    public partial class Repository
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "repositoryName")]
        public string? RepositoryName { get; set; }

        [Inject] private IRepositoryService? RepositoryService { get; set; }

        private Core.Models.Repository? RepositoryInstance { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (RepositoryService != null && RepositoryName != null)
            {
                RepositoryInstance = await RepositoryService.GetRepository(RepositoryName);
                StateHasChanged();
            }

            await base.OnParametersSetAsync();
        }
    }
}
