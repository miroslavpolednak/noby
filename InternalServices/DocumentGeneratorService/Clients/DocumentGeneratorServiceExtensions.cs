using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>>(new GrpcServiceUriSettingsServiceDiscovery<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(ServiceName));
        services.registerServices();
        return services;
    }

    public static IServiceCollection AddDocumentGeneratorService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>>(new GrpcServiceUriSettingsDirect<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>(serviceUrl));
        services.registerServices();
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentGeneratorServiceClient, DocumentGeneratorServiceClient>();

        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddTransient<ContextUserForwardingClientInterceptor>();

        var builder = services
            .AddGrpcClient<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<__Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();

        return services;
    }
}