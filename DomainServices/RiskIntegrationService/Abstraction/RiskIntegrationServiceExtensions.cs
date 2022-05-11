using CIS.DomainServicesSecurity.Abstraction;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.ClientFactory;

namespace DomainServices.RiskIntegrationService.Abstraction;

public static class RiskIntegrationServiceExtensions
{
    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, bool isInvalidCertificateAllowed)
        => services
            .AddCisServiceDiscovery(isInvalidCertificateAllowed)
            .registerUriSettings(isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    public static IServiceCollection AddRiskIntegrationService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<v1.IRipService>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<v1.IRipService>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:RiskIntegrationService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<v1.IRipService>(url ?? throw new ArgumentNullException("url", "RiskIntegrationService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        services.AddCisUserContextHelpers();

        // register storage services
        services.TryAddTransient<IRipServiceAbstraction, Services.RipService>();

        // exception handling
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(v1.IRipService)))
        {
            services
                .AddGrpcClientFromCisEnvironment<v1.IRipService>()
                .ConfigurePrimaryHttpMessageHandlerFromCisEnvironment<v1.IRipService>()
                .AddInterceptor<GenericClientExceptionInterceptor>()
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
