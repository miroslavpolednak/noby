using Microsoft.Extensions.Caching.Memory;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

internal class CachedConfigurationManager : IConfigurationManager
{
    private readonly IConfigurationManager _configurationManager;
    private readonly IMemoryCache _memoryCache;

    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public CachedConfigurationManager(IConfigurationManager configurationManager, IMemoryCache memoryCache, DataAggregatorServiceConfiguration configuration)
    {
        _configurationManager = configurationManager;
        _memoryCache = memoryCache;

        _cacheEntryOptions = new MemoryCacheEntryOptions { AbsoluteExpiration = DateTime.UtcNow.AddSeconds(configuration.CacheExpirationSeconds) };
    }

    public Task<Document.DocumentConfiguration> LoadDocumentConfiguration(Document.DocumentKey documentKey, CancellationToken cancellationToken)
    {
        return _memoryCache.GetOrCreateAsync(documentKey, LoadConfiguration)!;

        Task<Document.DocumentConfiguration> LoadConfiguration(ICacheEntry cacheEntry)
        {
            cacheEntry.SetOptions(_cacheEntryOptions);

            return _configurationManager.LoadDocumentConfiguration(documentKey, cancellationToken);
        }
    }

    public Task<EasForm.EasFormConfiguration> LoadEasFormConfiguration(EasForm.EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        return _memoryCache.GetOrCreateAsync(easFormKey, LoadConfiguration)!;

        Task<EasForm.EasFormConfiguration> LoadConfiguration(ICacheEntry cacheEntry)
        {
            cacheEntry.SetOptions(_cacheEntryOptions);

            return _configurationManager.LoadEasFormConfiguration(easFormKey, cancellationToken);
        }
    }

    private static readonly object _riskLoanApplicationKey = new();
    public Task<ConfigurationBase<RiskLoanApplication.RiskLoanApplicationSourceField>> LoadRiskLoanApplicationConfiguration(CancellationToken cancellationToken)
    {
        return _memoryCache.GetOrCreateAsync(_riskLoanApplicationKey, LoadConfiguration)!;

        Task<ConfigurationBase<RiskLoanApplication.RiskLoanApplicationSourceField>> LoadConfiguration(ICacheEntry cacheEntry)
        {
            cacheEntry.SetOptions(_cacheEntryOptions);

            return _configurationManager.LoadRiskLoanApplicationConfiguration(cancellationToken);
        }
    }
}