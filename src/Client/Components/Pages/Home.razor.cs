using ACRViewer.BlazorServer.Services.Interfaces;
using ACRViewer.BlazorServer.ViewModel;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Pages
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

        private void OnMenuItemChanged(ViewModel.TreeViewItemViewModel obj)
        {
            SelectedItem = obj;
            StateHasChanged();
        }
    }
}
