using StackExchange.Redis;

namespace CIS.Infrastructure.Caching.Tests;

public abstract class BaseGlobalCacheTest
{
    protected IGlobalCache getCache(string provider)
    {
        switch (provider)
        {
            case "redis":
                var redis = ConnectionMultiplexer.Connect(Constants.RedisConnectionString + ",allowAdmin=true");
                redis.GetServer(Constants.RedisConnectionString).FlushAllDatabases();
                return new Redis.RedisGlobalCache(redis, Constants.ApplicationKey, Constants.Environment);

            default:
                return new InMemory.InMemoryGlobalCache("Integration", Constants.ApplicationKey, Constants.Environment);
        }
    }
}
