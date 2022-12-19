using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.SalesArrangementService.Clients;
using __Services = DomainServices.SalesArrangementService.Clients.Services;
using __Contracts = DomainServices.SalesArrangementService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        services.TryAddTransient<ISalesArrangementServiceClient, __Services.SalesArrangementService>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddSalesArrangementService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<ISalesArrangementServiceClient, __Services.SalesArrangementService>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.SalesArrangementService.SalesArrangementServiceClient>(serviceUrl);
        return services;
    }
}
