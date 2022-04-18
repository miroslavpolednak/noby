using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using CIS.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace CIS.Infrastructure.Caching;

public static class DistributedCachingExtensions
{
    public static IServiceCollection AddCisDistributedCache(this IServiceCollection services)
    {
        services.AddSingleton<IDistributedCache>(provider =>
        {
            var environmentConfiguration = provider.GetService<ICisEnvironmentConfiguration>();
            if (environmentConfiguration == null)
                throw new CIS.Core.Exceptions.CisConfigurationNotRegisteredException();
            var cacheConfiguration = provider.GetService<ICisEnvironmentDistributedCacheConfiguration>();
            if (cacheConfiguration == null)
                throw new CIS.Core.Exceptions.CisConfigurationNotRegisteredException(nameof(ICisEnvironmentDistributedCacheConfiguration));

            switch (cacheConfiguration.CacheType)
            {
                case ICisEnvironmentDistributedCacheConfiguration.CacheTypes.Redis:
                    if (string.IsNullOrEmpty(cacheConfiguration.ConnectionString))
                        throw new CIS.Core.Exceptions.CisConfigurationNotRegisteredException("DistributedCache.ConnectionString");

                    var options = Options.Create<RedisCacheOptions>(new RedisCacheOptions
                    {
                        Configuration = cacheConfiguration.ConnectionString,
                        InstanceName = cacheConfiguration.KeyPrefix + environmentConfiguration.DefaultApplicationKey ?? "" + ":"
                    });

                    return new RedisCache(options);

                case ICisEnvironmentDistributedCacheConfiguration.CacheTypes.InMemory:
                    // tady se nezohledni nastaveni prefixu
                    // pokud bysme to potrebovali, bude se muset MemoryDistributedCache zabalito do wrapperu, ktery takovou funkcionalitu zajisti
                    var options2 = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
                    return new MemoryDistributedCache(options2);

                default: // kes neni zapnuta, co v tomto pripade delat? Bud defaultovat na InMemory nebo vyhodit vyjimku?
                    throw new NotImplementedException("Distribured cached configuration registered, but no cache type set");
            }
        });

        return services;
    }
}
