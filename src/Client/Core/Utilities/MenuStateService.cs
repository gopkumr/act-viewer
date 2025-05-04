using ACRViewer.BlazorServer.Core.Interface;

namespace ACRViewer.BlazorServer.Core.Utilities
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
