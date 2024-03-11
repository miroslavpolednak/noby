using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.SalesArrangementService.Clients;
using __Services = DomainServices.SalesArrangementService.Clients.Services;
using __Contracts = DomainServices.SalesArrangementService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DomainServices.SalesArrangementService.Clients.Services;

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
        services.AddScoped<ISalesArrangementServiceClient, __Services.SalesArrangementService>();
        services.AddScoped<IMaintananceService, __Services.MaintananceService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName, customServiceKey: "SAMaintananceServiceClient");
        return services;
    }

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IFlowSwitchManager, FlowSwitchManager>();

        services.AddScoped<ISalesArrangementServiceClient, __Services.SalesArrangementService>();
        services.AddScoped<IMaintananceService, __Services.MaintananceService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName, customServiceKey: "SAMaintananceServiceClient");
        return services;
    }
}
