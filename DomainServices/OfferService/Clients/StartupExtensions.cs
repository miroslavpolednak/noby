using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using __Contracts = DomainServices.OfferService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:OfferService";

    public static IServiceCollection AddOfferService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<OfferService.Clients.v1.IOfferServiceClient, OfferService.Clients.v1.OfferService >();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.OfferService.OfferServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddOfferService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<OfferService.Clients.v1.IOfferServiceClient, OfferService.Clients.v1.OfferService >();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.OfferService.OfferServiceClient>(serviceUrl);
        return services;
    }
}