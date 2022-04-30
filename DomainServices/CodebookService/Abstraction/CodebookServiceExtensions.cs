using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.DomainServices.Security.Abstraction;

namespace DomainServices.CodebookService.Abstraction;

public static class CodebookServiceExtensions
{
    /// <summary>
    /// Override for integration testing
    /// </summary>
    internal static IServiceCollection AddCodebookService(this IServiceCollection services, Action<IServiceProvider, Grpc.Net.ClientFactory.GrpcClientFactoryOptions> customConfiguration)
    {
        services
            .AddCodeFirstGrpcClient<Contracts.ICodebookService>(customConfiguration)
            .AddInterceptor<ExceptionInterceptor>()
            .AddInterceptor<AuthenticationInterceptor>();

        return services.registerServices();
    }

    public static IServiceCollection AddCodebookService(this IServiceCollection services, bool isInvalidCertificateAllowed)
        => services
            .AddCisServiceDiscovery(isInvalidCertificateAllowed)
            .registerUriSettings(isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddCodebookService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.ICodebookService>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
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
                return new GrpcServiceUriSettings<Contracts.ICodebookService>(url ?? throw new ArgumentNullException("url", "Codebook Service URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisUserContextHelpers();

        // register services
        services.TryAddTransient<ICodebookServiceAbstraction, CodebookService>();

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        // register cache
        services.TryAddSingleton(new AbstractionMemoryCache());

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.ICodebookService)))
        {
            services
                .AddCodeFirstGrpcClientFromCisEnvironment<Contracts.ICodebookService>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.ICodebookService>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
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
