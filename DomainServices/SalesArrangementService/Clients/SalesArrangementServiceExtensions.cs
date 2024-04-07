using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.SalesArrangementService.Clients;
using __Contracts = DomainServices.SalesArrangementService.Contracts;

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
        services.AddScoped<ISalesArrangementServiceClient, DomainServices.SalesArrangementService.Clients.v1.SalesArrangementService>();
        services.AddScoped<IMaintananceService, MaintananceService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName, customServiceKey: "SAMaintananceServiceClient");
        return services;
    }

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IFlowSwitchManager, FlowSwitchManager>();

        services.AddScoped<ISalesArrangementServiceClient, DomainServices.SalesArrangementService.Clients.v1.SalesArrangementService>();
        services.AddScoped<IMaintananceService, MaintananceService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName, customServiceKey: "SAMaintananceServiceClient");
        return services;
    }
}
