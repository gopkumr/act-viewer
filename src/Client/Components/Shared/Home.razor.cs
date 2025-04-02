using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Utilities;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Shared
{
    public partial class Home
    {
        private bool _loaded;

        [Inject] IMenuStateService? MenuState { get; set; }
        [Inject] private IACRService? AcrManager { get; set; }

        private TreeViewItemViewModel? SelectedItem { get; set; }

        protected override void OnInitialized()
        {
            _loaded = true;
            if (MenuState != null)
            {
                MenuState!.OnMenuItemChanged += OnMenuItemChanged;
            }
        }

        private void OnMenuItemChanged(TreeViewItemViewModel obj)
        {
            SelectedItem = obj;
            StateHasChanged();
        }
    }
}
