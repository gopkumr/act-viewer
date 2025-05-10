using ACRViewer.BlazorServer.Core.Interface;
using ACRViewer.BlazorServer.Features.Navigation.Services;
using ACRViewer.BlazorServer.Features.Repository.Services;
using ACRViewer.BlazorServer.Features.Tag.Services;

namespace ACRViewer.BlazorServer.Infrastructure
{
    public static class FeatureExtensions
    {
        public static IServiceCollection AddFeatureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
             .AddScoped<INavigationService, NavigationService>()
             .AddScoped<ISharedService, SharedService>()
             .AddScoped<IRepositoryService, RepositoryService>()
             .AddScoped<ITagService, TagService>()
            ;

            return serviceCollection;
        }
    }
}
