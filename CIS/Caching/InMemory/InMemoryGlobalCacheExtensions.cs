using CIS.Core.Configuration;
using CIS.Core.Exceptions;
using CIS.Core.Types;
using CIS.Infrastructure.Caching.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.Caching;

public static class InMemoryGlobalCacheExtensions
{
    public static IServiceCollection AddInMemoryGlobalCache(this IServiceCollection services, string? cacheName = null)
        => services.AddSingleton<IGlobalCache>(provider =>
        {
            try
            {
                var configuration = provider.GetRequiredService<ICisEnvironmentConfiguration>();
                return new InMemoryGlobalCache(cacheName ?? InMemoryGlobalCache.CisGlobalCacheName, new ApplicationKey(configuration.DefaultApplicationKey), new ApplicationEnvironmentName(configuration.EnvironmentName));
            }
            catch (InvalidOperationException)
            {
                throw new CisConfigurationNotRegisteredException();
            }
            catch
            {
                throw;
            }
        });

    public static IServiceCollection AddInMemoryGlobalCache<TCache>(this IServiceCollection services) where TCache : class
        => services.AddSingleton<IGlobalCache<TCache>>(provider =>
        {
            try
            {
                var configuration = provider.GetRequiredService<ICisEnvironmentConfiguration>();
                return new InMemoryGlobalCache<TCache>(new ApplicationKey(configuration.DefaultApplicationKey), new ApplicationEnvironmentName(configuration.EnvironmentName));
            }
            catch (InvalidOperationException)
            {
                throw new CisConfigurationNotRegisteredException();
            }
            catch
            {
                throw;
            }
        });

    public static IServiceCollection AddInMemoryGlobalCache(this IServiceCollection services, Action<InMemoryGlobalCacheOptions> options)
        => services.AddSingleton<IGlobalCache>(provider =>
        {
            var configuration = provider.GetService<ICisEnvironmentConfiguration>();
            var defaultOptions = new InMemoryGlobalCacheOptions(configuration?.EnvironmentName, configuration?.DefaultApplicationKey);
            options.Invoke(defaultOptions);

            return new InMemoryGlobalCache(defaultOptions.CacheName, new ApplicationKey(defaultOptions.ApplicationKey), new ApplicationEnvironmentName(defaultOptions.EnvironmentName));
        });
}
