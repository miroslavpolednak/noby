using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.CustomerService.Clients;

public static class CustomerServiceExtensions
{
    public static IServiceCollection AddCustomerService(this IServiceCollection services) =>
        services.TryAddGrpcClient<Contracts.V1.CustomerService.CustomerServiceClient>(a =>
        {
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.V1.CustomerService.CustomerServiceClient>("DS:CustomerService");
        }).RegisterServices();

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl) =>
        services.TryAddGrpcClient<Contracts.V1.CustomerService.CustomerServiceClient>(a =>
        {
            a.AddGrpcServiceUriSettings<Contracts.V1.CustomerService.CustomerServiceClient>(serviceUrl);
        }).RegisterServices();

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<ICustomerServiceClient, CustomerService>();

        services.AddGrpcClientFromCisEnvironment<Contracts.V1.CustomerService.CustomerServiceClient>();

        return services;
    }
}
