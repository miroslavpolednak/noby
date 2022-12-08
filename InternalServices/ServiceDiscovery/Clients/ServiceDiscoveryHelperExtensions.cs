using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.ServiceDiscovery.Clients;

public static class ServiceDiscoveryHelperExtensions
{
    public static IServiceCollection AddGrpcServiceUriSettingsFromServiceDiscovery<TService>(this IServiceCollection services, string serviceName)
        where TService : class
    {
        services.AddCisServiceDiscovery();
        
        services.AddSingleton(provider =>
        {
            string? url = provider
                .GetRequiredService<IDiscoveryServiceClient>()
                .GetServiceUrlSynchronously(new(serviceName), Contracts.ServiceTypes.Grpc);

            return new Infrastructure.gRPC.Configuration.GrpcServiceUriSettingsDirect<TService>(url ?? throw new ArgumentNullException(nameof(serviceName), $"{serviceName} URL can not be determined"));
        });

        return services;
    }
}
