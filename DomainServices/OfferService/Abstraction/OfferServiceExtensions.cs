using CIS.DomainServicesSecurity.Abstraction;
using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.OfferService.Abstraction;

public static class OfferServiceExtensions
{
    public static IServiceCollection AddOfferService(this IServiceCollection services, bool isInvalidCertificateAllowed)
        => services
            .AddCisServiceDiscovery(isInvalidCertificateAllowed)
            .registerUriSettings(isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();
    
    public static IServiceCollection AddOfferService(this IServiceCollection services, string serviceUrl, bool isInvalidCertificateAllowed)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.OfferService.OfferServiceClient>(serviceUrl, isInvalidCertificateAllowed)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services, bool isInvalidCertificateAllowed)
    {
        if (!services.Any(t => t.ServiceType == typeof(GrpcServiceUriSettings<Contracts.v1.OfferService.OfferServiceClient>)))
        {
            services.AddSingleton(provider =>
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("DS:OfferService"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Grpc)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                return new GrpcServiceUriSettings<Contracts.v1.OfferService.OfferServiceClient>(url ?? throw new ArgumentNullException("url", "OfferService URL can not be determined"), isInvalidCertificateAllowed);
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.TryAddTransient<IOfferServiceAbstraction, OfferService>();

        // exception handling
        services.TryAddSingleton<GenericClientExceptionInterceptor>();
        services.TryAddSingleton<ExceptionInterceptor>();
        services.TryAddSingleton<AuthenticationInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.OfferService.OfferServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.OfferService.OfferServiceClient>()
                .AddInterceptor<GenericClientExceptionInterceptor>()
                .AddInterceptor<ExceptionInterceptor>()
                .AddInterceptor<AuthenticationInterceptor>();
        }
        return services;
    }
}