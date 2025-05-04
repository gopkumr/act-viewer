using ACRViewer.BlazorServer.Core.Utilities;

namespace ACRViewer.BlazorServer.Core.Interface
{
    public interface IMenuStateService
    {
        event Action<TreeViewItemViewModel> OnMenuItemChanged;

        void SetSelectedItem(TreeViewItemViewModel item);
    }
}