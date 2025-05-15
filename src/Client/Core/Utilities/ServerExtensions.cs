using Arinco.BicepHub.App.Core.Interface;
using MudBlazor;
using MudBlazor.Services;

namespace Arinco.BicepHub.App.Core.Utilities
{
    public static class InfrastructureExtensions
    {
        public static WebApplicationBuilder AddClientServices(this WebApplicationBuilder builder)
        {
            var settings = new ACRSettings();
            builder.Configuration.GetSection(ACRSettings.SectionName).Bind(settings);

            builder
                .Services
                .AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddMudServices(configuration =>
                {
                    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
                    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
                    configuration.SnackbarConfiguration.ShowCloseIcon = false;
                })
            .AddScoped<IMenuStateService, MenuStateService>()
            .AddSingleton(settings)
            ;

            return builder;
        }

    }
}
