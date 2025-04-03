using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Core.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using MudBlazor;

namespace ACRViewer.BlazorServer.Components.Layout
{
    public partial class MainLayout
    {
        private MudTheme CurrentTheme { get; set; } = BlazorTheme.DefaultTheme;

        private bool _drawerOpen=true;

    }
}
