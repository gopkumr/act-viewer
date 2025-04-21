using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Utilities;
using Microsoft.AspNetCore.Components;

namespace ACRViewer.BlazorServer.Components.Shared
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
