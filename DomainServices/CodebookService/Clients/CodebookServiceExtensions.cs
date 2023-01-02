using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using DomainServices.CodebookService.Clients;
using __Contracts = DomainServices.CodebookService.Contracts;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class CodebookServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CodebookService";

    public static int DefaultAbsoluteCacheExpirationMinutes { get; private set; } = 10;

    public static IServiceCollection AddCodebookService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.ICodebookService>>(new GrpcServiceUriSettingsServiceDiscovery<__Contracts.ICodebookService>(ServiceName));
        services.registerServices();
        return services;
    }
    
    public static IServiceCollection AddCodebookService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddSingleton<IGrpcServiceUriSettings<__Contracts.ICodebookService>>(new GrpcServiceUriSettingsDirect<__Contracts.ICodebookService>(serviceUrl));
        services.registerServices();
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services, bool validateServiceCertificate = false)
    {
        // register services
        services.AddTransient<ICodebookServiceClients, CodebookService.Clients.CodebookService>();

        // register cache
        services.AddSingleton(new ClientsMemoryCache());

        var builder = services
            .AddCodeFirstGrpcClient<__Contracts.ICodebookService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<__Contracts.ICodebookService>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .AddCisCallCredentials();

        if (!validateServiceCertificate)
            builder.CisConfigureChannelWithoutCertificateValidation();

        return services;
    }
}
