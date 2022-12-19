using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Clients.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using __Contracts = CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices;

public static class DocumentGeneratorServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "CIS:DocumentGeneratorService";

    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<IDocumentGeneratorServiceClient, DocumentGeneratorServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(ServiceName);
        
        return services;
    }

    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<IDocumentGeneratorServiceClient, DocumentGeneratorServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(serviceUrl);

        return services;
    }
}