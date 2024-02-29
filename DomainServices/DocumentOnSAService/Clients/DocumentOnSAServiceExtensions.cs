using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using __Services = DomainServices.DocumentOnSAService.Clients.Services;
using __Contracts = DomainServices.DocumentOnSAService.Contracts;
using CIS.Infrastructure.gRPC;
using DomainServices.DocumentOnSAService.Clients;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class DocumentOnSAServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:DocumentOnSAService";

    public static IServiceCollection AddDocumentOnSAService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.TryAddTransient<IDocumentOnSAServiceClient, __Services.DocumentOnSAService>();
        services.TryAddTransient<IMaintananceService, __Services.MaintananceService>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.DocumentOnSAService.DocumentOnSAServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.DocumentOnSAService.DocumentOnSAServiceClient>(ServiceName, customServiceKey: "DOSAMaintananceServiceClient");
        return services;
    }

    public static IServiceCollection AddDocumentOnSAService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<IDocumentOnSAServiceClient, __Services.DocumentOnSAService>();
        services.TryAddTransient<IMaintananceService, __Services.MaintananceService>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.DocumentOnSAService.DocumentOnSAServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.DocumentOnSAService.DocumentOnSAServiceClient>(ServiceName, customServiceKey: "DOSAMaintananceServiceClient");
        return services;
    }
}
