namespace CIS.InternalServices.DataAggregatorService.Api;

public class DataAggregatorConfiguration
{
    public bool UseCacheForConfiguration { get; set; }

    public int CacheExpirationSeconds { get; set; }
}