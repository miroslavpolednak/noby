using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using __Contracts = DomainServices.DocumentArchiveService.Contracts;
using __Services = DomainServices.DocumentArchiveService.Clients.Services;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.DocumentArchiveService.Clients;

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

        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.DocumentArchiveService.DocumentArchiveServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddDocumentArchiveService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IDocumentArchiveServiceClient, __Services.DocumentArchiveService>();

        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.DocumentArchiveService.DocumentArchiveServiceClient>(serviceUrl);
        return services;
    }
}


