using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.SalesArrangementService.Clients;
using __Contracts = DomainServices.SalesArrangementService.Contracts;
using CIS.Infrastructure.Caching.Grpc;

namespace DomainServices;

public static class SalesArrangementServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:SalesArrangementService";

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services)
    {
        services.AddTransient<IFlowSwitchManager, FlowSwitchManager>();

        services.AddCisServiceDiscovery();

        services.AddGrpcClientResponseCaching<SalesArrangementService.Clients.v1.SalesArrangementServiceClient>(ServiceName);

        services.AddScoped<ISalesArrangementServiceClient, DomainServices.SalesArrangementService.Clients.v1.SalesArrangementServiceClient>();
        services.AddScoped<IMaintananceService, MaintananceService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName, customServiceKey: "SAMaintananceServiceClient");
        return services;
    }

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IFlowSwitchManager, FlowSwitchManager>();

        services.AddGrpcClientResponseCaching<SalesArrangementService.Clients.v1.SalesArrangementServiceClient>(ServiceName);

        services.AddScoped<ISalesArrangementServiceClient, DomainServices.SalesArrangementService.Clients.v1.SalesArrangementServiceClient>();
        services.AddScoped<IMaintananceService, MaintananceService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName, customServiceKey: "SAMaintananceServiceClient");
        return services;
    }
}
