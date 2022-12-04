using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.gRPC.Configuration;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;

namespace DomainServices.CodebookService.Clients;

public static class CodebookServiceExtensions
{
    public static int DefaultAbsoluteCacheExpirationMinutes { get; private set; } = 1;

    public static IServiceCollection AddCodebookService(this IServiceCollection services)
        => services
            .TryAddGrpcClient<Contracts.ICodebookService>(a =>
                a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.ICodebookService>("DS:CodebookService")
            .registerServices()
        );

    public static IServiceCollection AddCodebookService(this IServiceCollection services, string serviceUrl)
        => services
            .TryAddGrpcClient<Contracts.ICodebookService>(a =>
                a.AddGrpcServiceUriSettings<Contracts.ICodebookService>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services, bool validateServiceCertificate = false)
    {
        // register services
        services.AddTransient<ICodebookServiceClients, CodebookService>();

        // register cache
        services.AddSingleton(new ClientsMemoryCache());

        var builder = services
            .AddCodeFirstGrpcClient<Contracts.ICodebookService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<IGrpcServiceUriSettings<Contracts.ICodebookService>>();
                options.Address = serviceUri.ServiceUrl;
            })
            .AddCisCallCredentials();

        if (!validateServiceCertificate)
            builder.CisConfigureChannelWithoutCertificateValidation();

        return services;
    }
}
