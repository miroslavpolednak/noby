using CIS.Infrastructure.gRPC;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.OfferService.Abstraction;

public static class OfferServiceExtensions
{
    public static IServiceCollection AddOfferService(this IServiceCollection services)
        => services.TryAddGrpcClient<Contracts.v1.OfferService.OfferServiceClient>(a =>
            a.AddGrpcServiceUriSettingsFromServiceDiscovery<Contracts.v1.OfferService.OfferServiceClient>("DS:OfferService")
            .registerServices()
        );
    
    public static IServiceCollection AddOfferService(this IServiceCollection services, string serviceUrl)
        => services.TryAddGrpcClient<Contracts.v1.OfferService.OfferServiceClient>(a =>
            a.AddGrpcServiceUriSettings<Contracts.v1.OfferService.OfferServiceClient>(serviceUrl)
            .registerServices()
        );

    private static IServiceCollection registerServices(this IServiceCollection services)
    {
        // register storage services
        services.AddTransient<IOfferServiceAbstraction, OfferService>();

        // exception handling
        services.AddSingleton<ExceptionInterceptor>();

        services
                .AddGrpcClientFromCisEnvironment<Contracts.v1.OfferService.OfferServiceClient>()
                .AddInterceptor<ExceptionInterceptor>();

        return services;
    }
}