using ACRViewer.BlazorServer.Services.Interfaces;
using ACRViewer.BlazorServer.ViewModel;

namespace ACRViewer.BlazorServer.Services
{
    public class MenuStateService : IMenuStateService
    {
        private TreeViewItemViewModel? SelectedItem { get; set; }

        public event Action<TreeViewItemViewModel>? OnMenuItemChanged;

        public void SetSelectedItem(TreeViewItemViewModel item)
        {
            SelectedItem = item;
            OnMenuItemChanged?.Invoke(item);
        }
    }
}
