using CIS.Infrastructure.Caching.Grpc;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using DomainServices.HouseholdService.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using __Contracts = DomainServices.HouseholdService.Contracts;

namespace DomainServices;

public static class HouseholdServiceClientsStartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:HouseholdService";

    public static IServiceCollection AddHouseholdService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.TryAddSingleton<ICustomerChangeDataMerger, HouseholdService.Clients.Services.CustomerChangeDataMerger>();
        services.AddGrpcClientResponseCaching<HouseholdService.Clients.v1.HouseholdServiceClient>(ServiceName);

        services.TryAddTransient<HouseholdService.Clients.v1.IHouseholdServiceClient, HouseholdService.Clients.v1.HouseholdServiceClient>();
        services.TryAddTransient<HouseholdService.Clients.v1.ICustomerOnSAServiceClient, HouseholdService.Clients.v1.CustomerOnSAServiceClient>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddHouseholdService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddSingleton<ICustomerChangeDataMerger, HouseholdService.Clients.Services.CustomerChangeDataMerger>();

        services.TryAddTransient<HouseholdService.Clients.v1.IHouseholdServiceClient, HouseholdService.Clients.v1.HouseholdServiceClient>();
        services.TryAddTransient<HouseholdService.Clients.v1.ICustomerOnSAServiceClient, HouseholdService.Clients.v1.CustomerOnSAServiceClient>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        return services;
    }
}