using ACRViewer.BlazorServer.Components;
using ACRViewer.BlazorServer.Core.Utilities;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace ACRViewer.BlazorServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
                        .AddInteractiveServerComponents();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(options =>
                            {
                                builder.Configuration.GetSection("AzureAd").Bind(options);
                                options.Events.OnRedirectToIdentityProvider = context =>
                                {
                                    if (!context.Request.Path.StartsWithSegments("/MicrosoftIdentity/Account"))
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

        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews()
                        .AddMicrosoftIdentityUI();

        builder.Services.AddAuthorization();

        builder.Services.AddHttpContextAccessor();

        builder.AddClientServices();

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

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
