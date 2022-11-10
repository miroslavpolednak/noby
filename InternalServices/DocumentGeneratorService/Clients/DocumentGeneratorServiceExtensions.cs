using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

public static class DocumentGeneratorServiceExtensions
{
    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services) =>
        services.TryAddGrpcClient<Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(x =>
        {
            x.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>("CIS:DocumentGeneratorService");
        }).RegisterServices();

    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services, string serviceUrl) => services
        .TryAddGrpcClient<Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(x =>
        {
            x.AddGrpcServiceUriSettings<Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(serviceUrl);
        }).RegisterServices();

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentGeneratorServiceClient, DocumentGeneratorServiceClient>();

        services.AddGrpcClientFromCisEnvironment<Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>();

        return services;
    }
}