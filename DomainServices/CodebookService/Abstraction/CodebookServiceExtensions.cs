using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.CodebookService.Abstraction;

public static class CodebookServiceExtensions
{
    public static IServiceCollection AddCodebookService(this IServiceCollection services)
        => services
            .AddCisServiceDiscovery()
            .registerUriSettings()
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddCodebookService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.ICodebookService>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.ICodebookService>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:CodebookService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.ICodebookService>(url ?? throw new ArgumentNullException("url", "Codebook Service URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register services
        services.TryAddTransient<ICodebookServiceAbstraction, CodebookService>();

        // register cache
        services.TryAddSingleton(new AbstractionMemoryCache());

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.ICodebookService)))
        {
            services
                .AddCodeFirstGrpcClientFromCisEnvironment<Contracts.ICodebookService>();
        }
        return services;
    }

    private static IHttpClientBuilder AddCodeFirstGrpcClientFromCisEnvironment<TService>(this IServiceCollection services)
            where TService : class
    {
        return services.AddCodeFirstGrpcClient<TService>((provider, options) =>
        {
            var serviceUri = provider.GetRequiredService<GrpcServiceUriSettings<TService>>();
            options.Address = serviceUri.Url;
        });
    }
}
