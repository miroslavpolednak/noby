using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.SalesArrangementService.Abstraction;

public static class SalesArrangementServiceExtensions
{
    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>("DS:SalesArrangementService")
            .registerServices()
        );

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.AddTransient<ISalesArrangementServiceAbstraction, Services.SalesArrangementService>();
        services.AddTransient<ICustomerOnSAServiceAbstraction, Services.CustomerOnSAService>();
        services.AddTransient<IHouseholdServiceAbstraction, Services.HouseholdService>();

        services.AddGrpcClientFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();
        services.AddGrpcClientFromCisEnvironment<Contracts.v1.HouseholdService.HouseholdServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();
        services.AddGrpcClientFromCisEnvironment<Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();

        return services;
    }
}
