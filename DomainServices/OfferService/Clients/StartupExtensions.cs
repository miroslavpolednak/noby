using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.OfferService.Clients;
using __Services = DomainServices.OfferService.Clients.Services;
using __Contracts = DomainServices.OfferService.Contracts;

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
        services.AddTransient<IOfferServiceClient, __Services.OfferService>();
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.OfferService.OfferServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddOfferService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IOfferServiceClient, __Services.OfferService>();
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.OfferService.OfferServiceClient>(serviceUrl);
        return services;
    }
}