using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.SalesArrangementService.Clients;
using __Services = DomainServices.SalesArrangementService.Clients.Services;
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
        services.AddCisServiceDiscovery();
        services.AddTransient<ISalesArrangementServiceClient, __Services.SalesArrangementService>();
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<ISalesArrangementServiceClient, __Services.SalesArrangementService>();
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl);
        return services;
    }
}
