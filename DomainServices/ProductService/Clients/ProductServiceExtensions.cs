using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.ProductService.Clients;
using __Services = DomainServices.ProductService.Clients.Services;
using __Contracts = DomainServices.ProductService.Contracts;

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
        services.AddTransient<IProductServiceClient, __Services.ProductService>();
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.ProductService.ProductServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddProductService(this IServiceCollection services, string serviceUrl)
    {
        services.AddTransient<IProductServiceClient, __Services.ProductService>();
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.ProductService.ProductServiceClient>(serviceUrl);
        return services;
    }
}
