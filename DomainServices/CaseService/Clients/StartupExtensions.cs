using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
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
        services.TryAddScoped<CaseService.Clients.v1.ICaseServiceClient, CaseService.Clients.v1.CaseServiceClient>();
        services.TryAddScoped<CaseService.Clients.IMaintananceClient, CaseService.Clients.Services.MaintananceClient>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CaseService.CaseServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.CaseService.CaseServiceClient>(ServiceName, customServiceKey: "CaseMaintananceServiceClient");
        return services;
    }

#pragma warning disable CA1054 // URI-like parameters should not be strings
    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl)
#pragma warning restore CA1054 // URI-like parameters should not be strings
    {
        services.TryAddScoped<CaseService.Clients.v1.ICaseServiceClient, CaseService.Clients.v1.CaseServiceClient>();
        services.TryAddScoped<CaseService.Clients.IMaintananceClient, CaseService.Clients.Services.MaintananceClient>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.CaseService.CaseServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.CaseService.CaseServiceClient>(ServiceName, customServiceKey: "CaseMaintananceServiceClient");
        return services;
    }
}
