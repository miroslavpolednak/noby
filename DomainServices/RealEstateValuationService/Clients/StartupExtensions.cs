using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.RealEstateValuationService.Clients;
using __Services = DomainServices.RealEstateValuationService.Clients.Services;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:RealEstateValuationService";

    public static IServiceCollection AddRealEstateValuationService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<IRealEstateValuationServiceClient, __Services.RealEstateValuationServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddRealEstateValuationService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<IRealEstateValuationServiceClient, __Services.RealEstateValuationServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient>(serviceUrl);
        return services;
    }
}
