using ACRViewer.BlazorServer.Configuration;
using ACRViewer.BlazorServer.Managers;
using ACRViewer.BlazorServer.Services;
using ACRViewer.BlazorServer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using MudBlazor;
using MudBlazor.Services;

namespace ACRViewer.BlazorServer.Extensions
{
    public static class ServerExtensions
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
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IAuthenticationManager, Managers.AuthenticationManager>()
            .AddScoped<IMenuStateService, MenuStateService>()
            .AddSingleton(settings)
            .AddHttpClient<IACRService, ACRService>(client =>
              {
                  client.BaseAddress = new Uri(settings.BaseUrl);
              })
            ;

            return builder;
        }

    }
}
