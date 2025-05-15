using Arinco.BicepHub.App.Core.Utilities;
using Arinco.BicepHub.App.Features.Navigation.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Arinco.BicepHub.App.Components.Features.Navigation
{
    public partial class NavMenu
    {
        private List<TreeItemData<TreeViewItemViewModel>> Repositories { get; set; } = [];
        [Inject] private INavigationService? NavigationService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        private string _searchPhrase = "";

        private MudTreeView<TreeViewItemViewModel> _treeView;

        private bool _isLoading = false;

        private string? _loadingError = null;

        protected override async Task OnInitializedAsync()
        {
            _loadingError = null;
            try
            {
                _isLoading = true;
                await LoadRepositories();
            }
            catch (Azure.RequestFailedException reqEx)
            {
                _loadingError = $"Failed fetching repositories from Azure Container Registry: {reqEx.Message}";
                Console.WriteLine(reqEx.StackTrace);
            }
            catch (Exception ex)
            {
                _loadingError = $"Failed to loading repositories {ex.Message}";
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task LoadRepositories()
        {
            if (NavigationService == null)
            {
                throw new InvalidOperationException("ACR Manager not injected");
            }

            var repositories = await NavigationService.GetRepositoryNames() ?? [];
            var acrName = NavigationService.GetRepositoryName() ?? "No ACR Configured";

            Repositories.AddRange([.. repositories.Select(r => new TreeItemData<TreeViewItemViewModel>
            {
                Value = new TreeViewItemViewModel(TreeViewType.Repository, r, acrName),
                Icon = Icons.Material.Outlined.Folder,
                Expanded = false,
            })]);
        }
        public async Task<IReadOnlyCollection<TreeItemData<TreeViewItemViewModel>>> LoadServerData(TreeViewItemViewModel parentValue)
        {
            return parentValue.Type switch
            {
                TreeViewType.Repository => await LoadTags(parentValue.Name),
                _ => throw new InvalidOperationException("Invalid parent type")
            };
        }

        public EventCallback HandleItemClick(TreeViewItemViewModel selectsValue)
        {
            if (selectsValue != null)
            {
                switch (selectsValue.Type)
                {
                    case TreeViewType.Repository:
                        NavigationManager.NavigateTo($"/repository?repositoryName={selectsValue.Name}", false);
                        break;
                    case TreeViewType.Tag:
                        NavigationManager.NavigateTo($"/tag?repositoryName={selectsValue.ParentName}&tagName={selectsValue.Name}", false);
                        break;
                    default:
                        break;
                }
            }
            return EventCallback.Factory.Create(this, () => { });
        }

        private void OnItemsLoaded(TreeItemData<TreeViewItemViewModel> treeItemData, IReadOnlyCollection<TreeItemData<TreeViewItemViewModel>> children)
        {
            // here we store the server-loaded children in the treeItemData so that they are available in the InitialTreeItems
            // if you don't do this you loose already loaded children on next render update
            treeItemData.Children = children?.ToList();
        }

        private async Task<IReadOnlyCollection<TreeItemData<TreeViewItemViewModel>>> LoadTags(string repository)
        {
            if (NavigationService == null)
            {
                throw new InvalidOperationException("ACR Manager not injected");
            }
            var tags = await NavigationService.GetTagNames(repository) ?? [];
            return tags.Select(t => new TreeItemData<TreeViewItemViewModel>
            {
                Value = new TreeViewItemViewModel(TreeViewType.Tag, t, repository, false),
                Icon = Icons.Material.Filled.Label,
                Expanded = false,
                Expandable = false,
            }).ToList();
        }

        private async void OnTextChanged(string searchPhrase)
        {
            _searchPhrase = searchPhrase;
            await _treeView.FilterAsync();
        }

        private Task<bool> MatchesName(TreeItemData<TreeViewItemViewModel> item)
        {
            if (string.IsNullOrEmpty(_searchPhrase))
                return Task.FromResult(true);

            if (item?.Value?.Type == TreeViewType.Tag)
                return Task.FromResult((item?.Value?.ParentName ?? "").Contains(_searchPhrase, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult((item?.Value?.Name ?? "").Contains(_searchPhrase, StringComparison.OrdinalIgnoreCase));
        }
    }
}
