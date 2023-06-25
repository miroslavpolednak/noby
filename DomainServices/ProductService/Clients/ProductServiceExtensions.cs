using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.ProductService.Clients;
using __Services = DomainServices.ProductService.Clients.Services;
using __Contracts = DomainServices.ProductService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class ProductServiceExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:ProductService";

    public static IServiceCollection AddProductService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<IProductServiceClient, __Services.ProductServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.ProductService.ProductServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddProductService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<IProductServiceClient, __Services.ProductServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.ProductService.ProductServiceClient>(serviceUrl);
        return services;
    }
}
