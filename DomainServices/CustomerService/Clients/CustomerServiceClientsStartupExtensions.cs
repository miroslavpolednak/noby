using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class CustomerServiceClientsStartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CustomerService";

    public static IServiceCollection AddCustomerService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<CustomerService.Clients.v1.ICustomerServiceClient, CustomerService.Clients.v1.CustomerServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<CustomerService.Contracts.v1.CustomerService.CustomerServiceClient>(ServiceName);

        return services;
    }

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<CustomerService.Clients.v1.ICustomerServiceClient, CustomerService.Clients.v1.CustomerServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<CustomerService.Contracts.v1.CustomerService.CustomerServiceClient>(serviceUrl);

        return services;
    }
}
