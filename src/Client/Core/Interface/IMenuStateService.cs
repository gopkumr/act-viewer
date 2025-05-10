using Arinco.BicepHub.App.Core.Utilities;

namespace Arinco.BicepHub.App.Core.Interface
{
    public interface IMenuStateService
    {
        event Action<TreeViewItemViewModel> OnMenuItemChanged;

        void SetSelectedItem(TreeViewItemViewModel item);
    }
}