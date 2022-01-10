using CIS.Core.Configuration;
using CIS.Core.Exceptions;
using CIS.Core.Types;
using CIS.Infrastructure.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CIS.Infrastructure.Caching;

public static class RedisGlobalCacheExtensions
{
    public static IServiceCollection AddRedisGlobalCache<TCache>(this IServiceCollection services, Func<IServiceProvider, string> funcConnectionString, string? keyPrefix = null)
        where TCache : class
        => services.AddSingleton<IGlobalCache<TCache>>(provider => addFromConfiguration<TCache>(provider, funcConnectionString(provider), keyPrefix));

    public static IServiceCollection AddRedisGlobalCache<TCache>(this IServiceCollection services, string redisConnectionString, string? keyPrefix = null)
        where TCache : class
        => services.AddSingleton<IGlobalCache<TCache>>(provider => addFromConfiguration<TCache>(provider, redisConnectionString, keyPrefix));

    public static IServiceCollection AddRedisGlobalCache(this IServiceCollection services, Func<IServiceProvider, string> funcConnectionString, string? keyPrefix = null)
        => services.AddSingleton<IGlobalCache>(provider => addFromConfiguration(provider, funcConnectionString(provider), keyPrefix));

    public static IServiceCollection AddRedisGlobalCache(this IServiceCollection services, string redisConnectionString, string? keyPrefix = null)
        => services.AddSingleton<IGlobalCache>(provider => addFromConfiguration(provider, redisConnectionString, keyPrefix));

    public static IServiceCollection AddRedisGlobalCache(this IServiceCollection services, Action<RedisGlobalCacheOptions> options)
        => services.AddSingleton<IGlobalCache>(provider =>
        {
            var configuration = provider.GetService<ICisEnvironmentConfiguration>();
            var defaultOptions = new RedisGlobalCacheOptions(configuration?.EnvironmentName, configuration?.DefaultApplicationKey);
            options.Invoke(defaultOptions);

            // bez korektniho connection stringu vyhod chybu
            if (string.IsNullOrEmpty(defaultOptions.ConnectionString))
                throw new CisArgumentNullException(11, "Redis connection string is empty", "ConnectionString");

            return createMultiplexer(defaultOptions.ConnectionString, defaultOptions.ApplicationKey, defaultOptions.EnvironmentName, defaultOptions.KeyPrefix);
        });

    private static IGlobalCache<TCache> addFromConfiguration<TCache>(IServiceProvider provider, string redisConnectionString, string? keyPrefix = null)
         where TCache : class
    {
        try
        {
            var configuration = provider.GetRequiredService<ICisEnvironmentConfiguration>();
            return createMultiplexer<TCache>(redisConnectionString, configuration.DefaultApplicationKey, configuration.EnvironmentName, keyPrefix);
        }
        catch (InvalidOperationException)
        {
            throw new CisConfigurationNotRegisteredException();
        }
        catch
        {
            throw;
        }
    }

    private static IGlobalCache addFromConfiguration(IServiceProvider provider, string redisConnectionString, string? keyPrefix = null)
    {
        try
        {
            var configuration = provider.GetRequiredService<ICisEnvironmentConfiguration>();
            return createMultiplexer(redisConnectionString, configuration.DefaultApplicationKey, configuration.EnvironmentName, keyPrefix);
        }
        catch (InvalidOperationException)
        {
            throw new CisConfigurationNotRegisteredException();
        }
        catch
        {
            throw;
        }
    }

    private static RedisGlobalCache<TCache> createMultiplexer<TCache>(string redisConnectionString, string? applicationKey, string? environmentName, string? keyPrefix = null)
         where TCache : class
    {
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        return new RedisGlobalCache<TCache>(multiplexer, new ApplicationKey(applicationKey), new ApplicationEnvironmentName(environmentName), keyPrefix);
    }

    private static RedisGlobalCache createMultiplexer(string redisConnectionString, string? applicationKey, string? environmentName, string? keyPrefix = null)
    {
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        return new RedisGlobalCache(multiplexer, new ApplicationKey(applicationKey), new ApplicationEnvironmentName(environmentName), keyPrefix);
    }
}
