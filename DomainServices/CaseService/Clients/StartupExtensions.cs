using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.CaseService.Clients;
using __Services = DomainServices.CaseService.Clients.Services;
using __Contracts = DomainServices.CaseService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CaseService";

    public static IServiceCollection AddCaseService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddScoped<ICaseServiceClient, __Services.CaseServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CaseService.CaseServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddScoped<ICaseServiceClient, __Services.CaseServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.CaseService.CaseServiceClient>(serviceUrl);
        return services;
    }
}
