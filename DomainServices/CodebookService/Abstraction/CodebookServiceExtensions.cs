using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;

namespace DomainServices.CodebookService.Abstraction;

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

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register services
        services.AddTransient<ICodebookServiceAbstraction, CodebookService>();

        // register cache
        services.AddSingleton(new AbstractionMemoryCache());

        services
            .AddCodeFirstGrpcClient<Contracts.ICodebookService>((provider, options) =>
            {
                var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<Contracts.ICodebookService>>();
                options.Address = serviceUri.Url;
            })
            .CisConfigureChannel()
            .AddCisCallCredentials();

        return services;
    }
}
