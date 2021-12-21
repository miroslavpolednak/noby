namespace CIS.Infrastructure.Caching.Redis;

public sealed class RedisGlobalCacheOptions
{
    internal RedisGlobalCacheOptions(string? environmentName, string? applicationKey)
    {
        this.EnvironmentName = environmentName ?? "";
        this.ApplicationKey = applicationKey ?? "";
    }

    public string EnvironmentName { get; init; }

    public string ApplicationKey { get; init; }

    public string ConnectionString { get; set; } = "";

    public string KeyPrefix { get; init; } = "";
}
