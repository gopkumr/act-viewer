using Arinco.BicepHub.App.Components;
using Arinco.BicepHub.App.Core.Utilities;
using Arinco.BicepHub.App.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Extensions.Logging.Console;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Arinco.BicepHub.App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
                        .AddInteractiveServerComponents()
                        .AddMicrosoftIdentityConsentHandler()
                        ;

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(options =>
                            {
                                builder.Configuration.GetSection("AzureAd").Bind(options);
                                options.Events.OnRedirectToIdentityProvider = context =>
                                {
                                    if (!context.Request.Path.StartsWithSegments("/MicrosoftIdentity"))
                                    {
                                        // Redirect to custom login page instead of default
                                        context.Response.Redirect("/login?returnUrl=" + Uri.EscapeDataString(context.Properties.RedirectUri ?? "/"));
                                        context.HandleResponse(); // Prevent further processing
                                        return Task.CompletedTask;
                                    }
                                    return Task.CompletedTask;
                                };
                            })
                        .EnableTokenAcquisitionToCallDownstreamApi()
                        .AddInMemoryTokenCaches();

        builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.AccessDeniedPath = "/login?reason=access-denied";
        });

        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews()
                        .AddMicrosoftIdentityUI();

        builder.Services.AddAuthorization()
                        .AddHttpContextAccessor()
                        .AddLogging(builder => builder.AddConsole());

        builder.AddClientServices();
        builder.Services.AddInfrastructureServices();
        builder.Services.AddFeatureServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.MapStaticAssets();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.MapControllers(); // Required for MicrosoftIdentity controllers
        app.MapRazorPages();  // Required for MicrosoftIdentity Razor pages

        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
