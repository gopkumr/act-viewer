using ACRViewer.BlazorServer.Core.Utilities;
using MudBlazor;

namespace ACRViewer.BlazorServer.Components.Layout
{
    public partial class MainLayout
    {
        private MudTheme CurrentTheme { get; set; } = BlazorTheme.DefaultTheme;

        private bool _drawerOpen = true;

        private bool _isDarkMode = false;

    }
}
