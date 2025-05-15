using Arinco.BicepHub.App.Features.Navigation.Services;
using Arinco.BicepHub.App.Features.Repository.Services;
using Arinco.BicepHub.App.Features.Shared.Services;
using Arinco.BicepHub.App.Features.Tag.Services;

namespace Arinco.BicepHub.App.Features
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
