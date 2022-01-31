using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Security.InternalServices;
using CIS.Infrastructure.Caching;
using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

public static class ServiceDiscoveryExtensions
{
    /// <summary>
    /// Override for integration testing
    /// </summary>
    internal static IServiceCollection AddCisServiceDiscoveryTest(this IServiceCollection services, Action<Grpc.Net.ClientFactory.GrpcClientFactoryOptions> customConfiguration)
    {
        services
            .AddGrpcClient<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(customConfiguration)
            .AddInterceptor<ExceptionInterceptor>();

        return services.registerServices();
    }

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, bool isInvalidCertificateAllowed)
        => services
            .registerUriSettings(isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddCisServiceDiscovery(this IServiceCollection services, string discoveryServiceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(discoveryServiceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string url = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>()?.ServiceDiscoveryUrl ?? "";
                var logger = provider.GetService<ILoggerFactory>();
                if (logger != null) logger.CreateLogger<GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>>().DiscoveryServiceUrlFound(url);
                return new GrpcServiceUriSettings<Contracts.v1.DiscoveryService.DiscoveryServiceClient>(url, isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // mediatr
        services.AddMediatR(typeof(ServiceDiscoveryExtensions).Assembly);
        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();
        // abstraction svc
        services.TryAddTransient<IDiscoveryServiceAbstraction, DiscoveryService>();
        // def environment name
        services.TryAddSingleton(provider =>
        {
            var configuration = provider.GetService<Core.Configuration.ICisEnvironmentConfiguration>();
            return new EnvironmentNameProvider(configuration?.EnvironmentName);
        });
        // cache
        services.AddInMemoryGlobalCache<DiscoveryService>();
        // context user
        services.AddCisContextUser();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.DiscoveryService.DiscoveryServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.DiscoveryService.DiscoveryServiceClient>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<Contracts.v1.DiscoveryService.DiscoveryServiceClient>()
                .AddInterceptor<ExceptionInterceptor>();
        }
        return services;
    }
}
