using ACRViewer.BlazorServer.ViewModel;

namespace ACRViewer.BlazorServer.Services.Interfaces
{
    public interface IMenuStateService
    {
        event Action<TreeViewItemViewModel> OnMenuItemChanged;

        void SetSelectedItem(TreeViewItemViewModel item);
    }
}