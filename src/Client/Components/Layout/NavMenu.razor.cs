using ACRViewer.BlazorServer.Services.Interfaces;
using ACRViewer.BlazorServer.ViewModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ACRViewer.BlazorServer.Components.Layout
{
    public partial class NavMenu
    {
        private List<TreeItemData<TreeViewItemViewModel>> Repositories { get; set; } = [];
        [Inject] private IACRService? AcrManager { get; set; }

        [Inject] IMenuStateService? MenuState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadRepositories();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task LoadRepositories()
        {
            if (AcrManager == null)
            {
                throw new InvalidOperationException("ACR Manager not injected");
            }

            var repositories = await AcrManager.GetAllRepositories() ?? [];
            var acrName = AcrManager.GetACRName() ?? "No ACR Configured";

            var respositories = repositories.Select(r => new TreeItemData<TreeViewItemViewModel>
            {
                Value = new TreeViewItemViewModel(TreeViewType.Repository, r.Name, acrName),
                Icon = Icons.Material.Filled.Folder,
                Expanded = false,
            }).ToList();

            Repositories.Add(new TreeItemData<TreeViewItemViewModel>
            {
                Value = new TreeViewItemViewModel(TreeViewType.ACR, acrName, string.Empty),
                Icon = Icons.Material.Filled.Apps,
                Expanded = true,
                Children = respositories
            });
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
            MenuState?.SetSelectedItem(selectsValue);
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
            if (AcrManager == null)
            {
                throw new InvalidOperationException("ACR Manager not injected");
            }
            var tags = await AcrManager.GetTags(repository) ?? [];
            return tags.Select(t => new TreeItemData<TreeViewItemViewModel>
            {
                Value = new TreeViewItemViewModel(TreeViewType.Tag, t.Name, repository, false),
                Icon = Icons.Material.Filled.Label,
                Expanded = false,
                Expandable = false,
            }).ToList();
        }
    }
}
