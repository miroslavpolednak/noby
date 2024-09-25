using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using __Contracts = DomainServices.OfferService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class OfferServiceClientsStartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:OfferService";

    public static IServiceCollection AddOfferService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();

        services.TryAddTransient<OfferService.Clients.v1.IOfferServiceClient, OfferService.Clients.v1.OfferServiceClient>();
        services.TryAddTransient<OfferService.Clients.Interfaces.IMaintananceServiceClient, OfferService.Clients.Services.MaintananceServiceClient>();

        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.OfferService.OfferServiceClient>(ServiceName);
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.OfferService.OfferServiceClient>(ServiceName, customServiceKey: "OFMaintananceServiceClient");
        return services;
    }

#pragma warning disable CA1054 // URI-like parameters should not be strings
    public static IServiceCollection AddOfferService(this IServiceCollection services, string serviceUrl)
#pragma warning restore CA1054 // URI-like parameters should not be strings
    {
        services.TryAddTransient<OfferService.Clients.v1.IOfferServiceClient, OfferService.Clients.v1.OfferServiceClient>();
        services.TryAddTransient<OfferService.Clients.Interfaces.IMaintananceServiceClient, OfferService.Clients.Services.MaintananceServiceClient>();

        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.OfferService.OfferServiceClient>(serviceUrl);
        services.TryAddCisGrpcClientUsingUrl<__Contracts.MaintananceService.MaintananceServiceClient, __Contracts.v1.OfferService.OfferServiceClient>(ServiceName, customServiceKey: "OFMaintananceServiceClient");
        return services;
    }
}