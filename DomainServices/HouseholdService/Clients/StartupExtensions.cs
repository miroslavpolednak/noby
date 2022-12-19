using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.HouseholdService.Clients;
using __Services = DomainServices.HouseholdService.Clients.Services;
using __Contracts = DomainServices.HouseholdService.Contracts;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:HouseholdService";

    public static IServiceCollection AddHouseholdService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.TryAddTransient<IHouseholdServiceClient, __Services.HouseholdService>();
        services.TryAddTransient<ICustomerOnSAServiceClient, __Services.CustomerOnSAService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddHouseholdService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<IHouseholdServiceClient, __Services.HouseholdService>();
        services.TryAddTransient<ICustomerOnSAServiceClient, __Services.CustomerOnSAService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        return services;
    }
}
