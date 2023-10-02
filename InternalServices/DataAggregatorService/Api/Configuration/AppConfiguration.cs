namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

public class AppConfiguration
{
    public bool UseCacheForConfiguration { get; set; }

    public int CacheExpirationSeconds { get; set; }
}