using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Clients.Services;
using Microsoft.Extensions.DependencyInjection;
using __Contracts = CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices;

public static class DocumentGeneratorServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:DocumentGeneratorService";

    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.AddTransient<IDocumentGeneratorServiceClient, DocumentGeneratorServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(ServiceName);
        
        return services;
    }

    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IDocumentGeneratorServiceClient, DocumentGeneratorServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(ServiceName);

        return services;
    }
}