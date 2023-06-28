using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.CodebookService.Clients;
using __Services = DomainServices.CodebookService.Clients.Services;
using __Contracts = DomainServices.CodebookService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:CodebookService";

    public const int DefaultAbsoluteCacheExpirationMinutes = 10;

    public static IServiceCollection AddCodebookService(this IServiceCollection services)
    {
        // register cache
        services.AddSingleton(new ClientsMemoryCache());

        services.AddCisServiceDiscovery();
        services.TryAddTransient<ICodebookServiceClient, __Services.CodebookService>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.CodebookService.CodebookServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddCodebookService(this IServiceCollection services, string serviceUrl)
    {
        // register cache
        services.AddSingleton(new ClientsMemoryCache());

        services.TryAddTransient<ICodebookServiceClient, __Services.CodebookService>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.CodebookService.CodebookServiceClient>(serviceUrl);
        return services;
    }
}
