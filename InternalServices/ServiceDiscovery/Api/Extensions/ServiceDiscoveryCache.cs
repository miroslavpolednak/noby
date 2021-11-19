using CIS.Infrastructure.Caching;

namespace CIS.InternalServices.ServiceDiscovery.Api;

public static class ServiceDiscoveryCache
{
    /// <summary>
    /// Registrace kesovaci sluzby podle nastaveni v appsettings.json
    /// </summary>
    public static IServiceCollection AddServiceDiscoveryCache(this IServiceCollection services, AppConfiguration appConfiguration)
    {
        services.AddHttpContextAccessor();

        switch (appConfiguration.CacheType)
        {
            case CacheTypes.InMemory:
                services.AddInMemoryGlobalCache("ServiceDiscoveryCache");
                break;

            case CacheTypes.Redis:
                if (string.IsNullOrEmpty(appConfiguration.CacheConnectionString))
                    throw new ArgumentNullException("CacheConnectionString", "Redis connection string for Service Discovery Global Cache must be defined");

                services.AddRedisGlobalCache(appConfiguration.CacheConnectionString);
                break;
        }

        return services;
    }
}
