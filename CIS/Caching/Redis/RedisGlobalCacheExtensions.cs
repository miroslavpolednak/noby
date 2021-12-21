using CIS.Core.Configuration;
using CIS.Core.Exceptions;
using CIS.Core.Types;
using CIS.Infrastructure.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CIS.Infrastructure.Caching;

public static class RedisGlobalCacheExtensions
{
    public static IServiceCollection AddRedisGlobalCache(this IServiceCollection services, Func<IServiceProvider, string> funcConnectionString, string? keyPrefix = null)
        => services.AddSingleton<IGlobalCache>(provider =>
        {
            try
            {
                string redisConnectionString = funcConnectionString(provider);
                var configuration = provider.GetRequiredService<ICisEnvironmentConfiguration>();
                var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
                return new RedisGlobalCache(multiplexer, new ApplicationKey(configuration.DefaultApplicationKey), new ApplicationEnvironmentName(configuration.EnvironmentName), keyPrefix);
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

    public static IServiceCollection AddRedisGlobalCache(this IServiceCollection services, string redisConnectionString, string? keyPrefix = null)
        => services.AddSingleton<IGlobalCache>(provider =>
        {
            try
            {
                var configuration = provider.GetRequiredService<ICisEnvironmentConfiguration>();
                var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
                return new RedisGlobalCache(multiplexer, new ApplicationKey(configuration.DefaultApplicationKey), new ApplicationEnvironmentName(configuration.EnvironmentName), keyPrefix);
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

    public static IServiceCollection AddRedisGlobalCache(this IServiceCollection services, Action<RedisGlobalCacheOptions> options)
        => services.AddSingleton<IGlobalCache>(provider =>
        {
            var configuration = provider.GetService<ICisEnvironmentConfiguration>();
            var defaultOptions = new RedisGlobalCacheOptions(configuration?.EnvironmentName, configuration?.DefaultApplicationKey);
            options.Invoke(defaultOptions);

            // bez korektniho connection stringu vyhod chybu
            if (string.IsNullOrEmpty(defaultOptions.ConnectionString))
                throw new CisArgumentNullException(11, "Redis connection string is empty", "ConnectionString");

            var multiplexer = ConnectionMultiplexer.Connect(defaultOptions.ConnectionString);
            return new RedisGlobalCache(multiplexer, new ApplicationKey(defaultOptions.ApplicationKey), new ApplicationEnvironmentName(defaultOptions.EnvironmentName), defaultOptions.KeyPrefix);
        });
}
