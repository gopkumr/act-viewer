using MudBlazor;

namespace Arinco.BicepHub.App.Core.Utilities
{
    public class BlazorTheme
    {
        private static Typography DefaultTypography = new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = ".875rem",
                LetterSpacing = ".01071em"
            },
            H1 = new H1Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "6rem",
                LetterSpacing = "-.01562em"
            },
            H2 = new H2Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "3.75rem",
                LetterSpacing = "-.00833em"
            },
            H3 = new H3Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "3rem",
                LetterSpacing = "0"
            },
            H4 = new H4Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "2.125rem",
                LetterSpacing = ".00735em"
            },
            H5 = new H5Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "1.5rem",
                LetterSpacing = "0"
            },
            H6 = new H6Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "1.25rem",
                LetterSpacing = ".0075em"
            },
            Button = new ButtonTypography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = ".875rem",
                LetterSpacing = ".02857em"
            },
            Body1 = new Body1Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = "1rem",
                LetterSpacing = ".00938em"
            },
            Body2 = new Body2Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = ".875rem",
                LetterSpacing = ".01071em"
            },
            Caption = new CaptionTypography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = ".75rem",
                LetterSpacing = ".03333em"
            },
            Subtitle2 = new Subtitle2Typography()
            {
                FontFamily = new[] { "Montserrat", "Sans-serif" },
                FontSize = ".875rem",
                LetterSpacing = ".00714em"
            }
        };

        private static LayoutProperties DefaultLayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "3px"
        };

        public static MudTheme DefaultTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#1E88E5",
                AppbarBackground = "#1E88E5",
                Background = Colors.Gray.Lighten5,
                DrawerBackground = "#FFF",
                DrawerText = "rgba(0,0,0, 0.7)",
                Success = "#007E33"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };

        public static MudTheme DarkTheme = new()
        {

            PaletteDark = new PaletteDark()
            {
                Primary = "#1E88E5",
                Success = "#007E33",
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGray = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#373740",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                DrawerIcon = "rgba(255,255,255, 0.50)"
            },
            Typography = DefaultTypography,
            LayoutProperties = DefaultLayoutProperties
        };
    }
}