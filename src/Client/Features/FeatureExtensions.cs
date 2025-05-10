using Arinco.BicepHub.App.Core.Interface;
using Arinco.BicepHub.App.Features.Navigation.Services;
using Arinco.BicepHub.App.Features.Repository.Services;
using Arinco.BicepHub.App.Features.Tag.Services;

namespace Arinco.BicepHub.App.Infrastructure
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
