using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.ClientFactory;
using _Contracts = DomainServices.DocumentArchiveService.Contracts;

namespace DomainServices;

public static class DocumentArchiveServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CodebookService";

    public static IServiceCollection AddDocumentArchiveService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddSingleton<IGrpcServiceUriSettings<_Contracts.IDocumentArchiveService>>(new GrpcServiceUriSettingsServiceDiscovery<_Contracts.IDocumentArchiveService>(ServiceName));
        services.registerServices();
        return services;
    }

    public static IServiceCollection AddDocumentArchiveService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddSingleton<IGrpcServiceUriSettings<_Contracts.IDocumentArchiveService>>(new GrpcServiceUriSettingsDirect<_Contracts.IDocumentArchiveService>(serviceUrl));
        services.registerServices();
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddSingleton<GenericClientExceptionInterceptor>();
        services.AddScoped<ContextUserForwardingClientInterceptor>();

        services.register<_Contracts.IDocumentArchiveService, DocumentArchiveService.Clients.IDocumentArchiveServiceClient, DocumentArchiveService.Clients.Services.DocumentArchiveServiceClient>();

        return services;
    }

    static void register<IService, IAbstraction, TAbstraction>(this IServiceCollection services)
        where IService : class
        where IAbstraction : class
        where TAbstraction : class
        => services
            .AddTransient(typeof(IAbstraction), typeof(TAbstraction))
            .AddCodeFirstGrpcClient<IService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<_Contracts.IDocumentArchiveService>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .CisConfigureChannelWithoutCertificateValidation()
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true)
            .AddInterceptor<GenericClientExceptionInterceptor>()
            .AddInterceptor<ContextUserForwardingClientInterceptor>()
            .AddCisCallCredentials();
}
