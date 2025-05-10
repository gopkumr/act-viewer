using Arinco.BicepHub.App.Core.Utilities;
using MudBlazor;

namespace Arinco.BicepHub.App.Components.Layout
{
    public partial class MainLayout
    {
        private MudTheme CurrentTheme { get; set; } = BlazorTheme.DefaultTheme;

        private bool _drawerOpen = true;

        private bool _isDarkMode = false;

    }
}
