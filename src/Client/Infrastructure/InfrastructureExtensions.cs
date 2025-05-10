using Arinco.BicepHub.App.Core.Interface;

namespace Arinco.BicepHub.App.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
             .AddScoped<ITokenService, TokenService>()
             .AddScoped<IAuthenticationManager, AuthenticationManager>()
             .AddHttpClient<IContainerRegistryClientService, ContainerRegistryClientService>()
            ;

            return serviceCollection;
        }

    }
}
