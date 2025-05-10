using Arinco.BicepHub.App.Core.Interface;

namespace Arinco.BicepHub.App.Core.Utilities
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
