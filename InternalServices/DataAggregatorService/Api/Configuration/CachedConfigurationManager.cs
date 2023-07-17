using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;
using Microsoft.Extensions.Caching.Memory;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

internal class CachedConfigurationManager : IConfigurationManager
{
    private readonly IConfigurationManager _configurationManager;
    private readonly IMemoryCache _memoryCache;

    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public CachedConfigurationManager(IConfigurationManager configurationManager, IMemoryCache memoryCache, DataAggregatorConfiguration configuration)
    {
        _configurationManager = configurationManager;
        _memoryCache = memoryCache;

        _cacheEntryOptions = new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(configuration.CacheExpirationSeconds) };
    }

    public Task<DocumentConfiguration> LoadDocumentConfiguration(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        return _memoryCache.GetOrCreateAsync(documentKey, LoadConfiguration)!;

        Task<DocumentConfiguration> LoadConfiguration(ICacheEntry cacheEntry)
        {
            cacheEntry.SetOptions(_cacheEntryOptions);

            return _configurationManager.LoadDocumentConfiguration(documentKey, cancellationToken);
        }
    }

    public Task<EasFormConfiguration> LoadEasFormConfiguration(EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        return _memoryCache.GetOrCreateAsync(easFormKey, LoadConfiguration)!;

        Task<EasFormConfiguration> LoadConfiguration(ICacheEntry cacheEntry)
        {
            cacheEntry.SetOptions(_cacheEntryOptions);

            return _configurationManager.LoadEasFormConfiguration(easFormKey, cancellationToken);
        }
    }

    private static readonly object _riskLoanApplicationKey = new();
    public Task<RiskLoanApplicationConfiguration> LoadRiskLoanApplicationConfiguration(CancellationToken cancellationToken)
    {
        return _memoryCache.GetOrCreateAsync(_riskLoanApplicationKey, LoadConfiguration)!;

        Task<RiskLoanApplicationConfiguration> LoadConfiguration(ICacheEntry cacheEntry)
        {
            cacheEntry.SetOptions(_cacheEntryOptions);

            return _configurationManager.LoadRiskLoanApplicationConfiguration(cancellationToken);
        }
    }
}