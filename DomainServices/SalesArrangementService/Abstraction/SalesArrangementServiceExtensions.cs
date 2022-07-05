using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.SalesArrangementService.Abstraction;

public static class SalesArrangementServiceExtensions
{
    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services)
    => services
        .AddCisServiceDiscovery()
        .registerUriSettings()
        .registerServices()
        .registerGrpcServices();

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl)
            .AddGrpcServiceUriSettings<Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient>(serviceUrl)
            .AddGrpcServiceUriSettings<Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider
                    .GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetServiceUrlSynchronously(new("DS:SalesArrangementService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc);
                return new GrpcServiceUriSettings<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(url ?? throw new ArgumentNullException("url", "SalesArrangementService URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.TryAddTransient<ISalesArrangementServiceAbstraction, Services.SalesArrangementService>();
        services.TryAddTransient<ICustomerOnSAServiceAbstraction, Services.CustomerOnSAService>();
        services.TryAddTransient<IHouseholdServiceAbstraction, Services.HouseholdService>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.SalesArrangementService.SalesArrangementServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.SalesArrangementService.SalesArrangementServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();
        }
        
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.HouseholdService.HouseholdServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.HouseholdService.HouseholdServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();
        }
        
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>();
        }
        return services;
    }
}
