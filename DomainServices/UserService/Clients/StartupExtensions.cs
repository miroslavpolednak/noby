using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.UserService.Clients;
using __Services = DomainServices.UserService.Clients.Services;
using __Contracts = DomainServices.UserService.Contracts;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:UserService";

    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.UserService.UserServiceClient>(ServiceName);
        services.TryAddTransient<IUserServiceClient, __Services.UserService>();
        tryAddDistributedCacheProvider(services);

        return services;
    }

    public static IServiceCollection AddUserService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v1.UserService.UserServiceClient>(serviceUrl);
        services.TryAddTransient<IUserServiceClient, __Services.UserService>();
        tryAddDistributedCacheProvider(services);

        return services;
    }

    private static void tryAddDistributedCacheProvider(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var cacheInstance = provider.GetService<IDistributedCache>();

            if (cacheInstance is not null)
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var doNotUseCache = configuration.GetValue<bool?>($"{CIS.Core.CisGlobalConstants.DomainServicesClientsConfigurationSectionName}:UserService:DisableDistributedCache") ?? false;
                if (!doNotUseCache)
                {
                    return new UserServiceClientCacheProvider
                    {
                        DistributedCacheInstance = cacheInstance,
                        UseDistributedCache = true,
                    };
                }
            }

            return new UserServiceClientCacheProvider();
        });
    }
}
