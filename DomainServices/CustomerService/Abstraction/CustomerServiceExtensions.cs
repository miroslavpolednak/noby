using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.CustomerService.Abstraction;

public static class CustomerServiceExtensions
{
    public static IServiceCollection AddCustomerService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.V1.CustomerService.CustomerServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.V1.CustomerService.CustomerServiceClient>("DS:CustomerService")
            .registerServices()
        );

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.V1.CustomerService.CustomerServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.AddTransient<ICustomerServiceAbstraction, CustomerService>();

        services.AddGrpcClientFromCisEnvironment<Contracts.V1.CustomerService.CustomerServiceClient>();

        return services;
    }
}
