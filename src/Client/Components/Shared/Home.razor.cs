using Arinco.BicepHub.App.Core.Utilities;

namespace Arinco.BicepHub.App.Components.Shared
{
    public partial class Home
    {
        private bool _loaded;

        private TreeViewItemViewModel? SelectedItem { get; set; }

        protected override void OnInitialized()
        {
            _loaded = true;
        }

    }
}
