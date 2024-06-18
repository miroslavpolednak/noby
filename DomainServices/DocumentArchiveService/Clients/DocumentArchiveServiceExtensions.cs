using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using __Contracts = DomainServices.DocumentArchiveService.Contracts;
using __Services = DomainServices.DocumentArchiveService.Clients.Services;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.DocumentArchiveService.Clients;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class DocumentArchiveServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:DocumentArchiveService";

    public static IServiceCollection AddDocumentArchiveService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.AddTransient<IDocumentArchiveServiceClient, __Services.DocumentArchiveService>();
        services.AddTransient<IMaintananceService, __Services.MaintananceService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.DocumentArchiveService.DocumentArchiveServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.DocumentArchiveService.DocumentArchiveServiceClient>(ServiceName, customServiceKey: "DASMaintananceServiceClient");
        return services;
    }

    public static IServiceCollection AddDocumentArchiveService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IDocumentArchiveServiceClient, __Services.DocumentArchiveService>();
        services.TryAddTransient<IMaintananceService, __Services.MaintananceService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.DocumentArchiveService.DocumentArchiveServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.DocumentArchiveService.DocumentArchiveServiceClient>(ServiceName, customServiceKey: "DASMaintananceServiceClient");
        return services;
    }
}


