using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CustomerService.Abstraction;

public static class CustomerServiceExtensions
{
    public static IServiceCollection AddCustomerService(this IServiceCollection services)
        => services
            .AddCisServiceDiscovery()
            .registerUriSettings()
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("DS:CustomerService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc);
                return new GrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>(url ?? throw new ArgumentNullException("url", "CustomerService URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.TryAddTransient<ICustomerServiceAbstraction, CustomerService>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.V1.CustomerService.CustomerServiceClient)))
        {
            services
            .AddGrpcClientFromCisEnvironment<Contracts.V1.CustomerService.CustomerServiceClient>();
        }
        return services;
    }
}
