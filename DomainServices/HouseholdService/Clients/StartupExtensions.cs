using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using DomainServices.HouseholdService.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using __Contracts = DomainServices.HouseholdService.Contracts;
using __Services = DomainServices.HouseholdService.Clients.Services;

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

        services.TryAddSingleton<ICustomerChangeDataMerger, __Services.CustomerChangeDataMerger>();

        services.TryAddTransient<IHouseholdServiceClient, __Services.HouseholdService>();
        services.TryAddTransient<ICustomerOnSAServiceClient, __Services.CustomerOnSAService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddHouseholdService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddSingleton<ICustomerChangeDataMerger, __Services.CustomerChangeDataMerger>();

        services.TryAddTransient<IHouseholdServiceClient, __Services.HouseholdService>();
        services.TryAddTransient<ICustomerOnSAServiceClient, __Services.CustomerOnSAService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        return services;
    }
}