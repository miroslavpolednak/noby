using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.CustomerService.Clients;
using __Services = DomainServices.CustomerService.Clients.Services;
using __Contracts = DomainServices.CustomerService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class CustomerServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CustomerService";

    public static IServiceCollection AddCustomerService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<ICustomerServiceClient, __Services.CustomerService>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.V1.CustomerService.CustomerServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<ICustomerServiceClient, __Services.CustomerService>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.V1.CustomerService.CustomerServiceClient>(serviceUrl);
        return services;
    }
}
