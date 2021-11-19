namespace CIS.Infrastructure.Caching.InMemory;

public sealed class InMemoryGlobalCacheOptions
{
    internal InMemoryGlobalCacheOptions(string? environmentName, string? applicationKey)
    {
        this.EnvironmentName = environmentName ?? "";
        this.ApplicationKey = applicationKey ?? "";
    }

    public string CacheName { get; set; } = "";

    public string EnvironmentName { get; init; }

    public string ApplicationKey { get; init; }
}
