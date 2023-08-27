namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

public class DataAggregatorServiceConfiguration
{
    public bool UseCacheForConfiguration { get; set; }

    public int CacheExpirationSeconds { get; set; }
}