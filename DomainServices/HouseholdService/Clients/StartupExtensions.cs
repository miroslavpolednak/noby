using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.HouseholdService.Clients;

public static class StartupExtensions
{
    public static IServiceCollection AddHouseholdService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.HouseholdService.HouseholdServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.v1.HouseholdService.HouseholdServiceClient>("DS:HouseholdService")
            .registerServices()
        );

    public static IServiceCollection AddHouseholdService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.v1.HouseholdService.HouseholdServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.AddScoped<IHouseholdServiceClient, Services.HouseholdService>();
        services.AddScoped<ICustomerOnSAServiceClient, Services.CustomerOnSAService>();

        services.AddGrpcClientFromCisEnvironment<Contracts.v1.HouseholdService.HouseholdServiceClient, Contracts.v1.HouseholdService.HouseholdServiceClient>();
        services.AddGrpcClientFromCisEnvironment<Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, Contracts.v1.HouseholdService.HouseholdServiceClient>();

        return services;
    }
}
