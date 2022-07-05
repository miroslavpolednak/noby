using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices.OfferService.Abstraction;

public static class OfferServiceExtensions
{
    public static IServiceCollection AddOfferService(this IServiceCollection services)
        => services
            .AddCisServiceDiscovery()
            .registerUriSettings()
            .registerServices()
            .registerGrpcServices();
    
    public static IServiceCollection AddOfferService(this IServiceCollection services, string serviceUrl)
        => services
            .AddGrpcServiceUriSettings<Contracts.v1.OfferService.OfferServiceClient>(serviceUrl)
            .registerServices()
            .registerGrpcServices();

    private static IServiceCollection registerUriSettings(this IServiceCollection services)
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
                return new GrpcServiceUriSettings<Contracts.v1.OfferService.OfferServiceClient>(url ?? throw new ArgumentNullException("url", "OfferService URL can not be determined"));
            });
        }
        return services;
    }

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.TryAddTransient<IOfferServiceAbstraction, OfferService>();

        // exception handling
        services.TryAddSingleton<ExceptionInterceptor>();

        return services;
    }

    private static IServiceCollection registerGrpcServices(this IServiceCollection services)
    {
        if (!services.Any(t => t.ServiceType == typeof(Contracts.v1.OfferService.OfferServiceClient)))
        {
            services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.OfferService.OfferServiceClient>()
                .AddInterceptor<ExceptionInterceptor>();
        }
        return services;
    }
}