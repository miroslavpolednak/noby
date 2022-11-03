using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Clients;
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
        
        services.AddGrpcClientFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();
        
        return services;
    }
}
