using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.CaseService.Clients;
using __Services = DomainServices.CaseService.Clients.Services;
using __Contracts = DomainServices.CaseService.Contracts;

namespace DomainServices;

public static class CaseServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CaseService";

    public static IServiceCollection AddCaseService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.AddTransient<ICaseServiceClient, __Services.CaseService>();
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CaseService.CaseServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<ICaseServiceClient, __Services.CaseService>();
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.CaseService.CaseServiceClient>(serviceUrl);
        return services;
    }
}
