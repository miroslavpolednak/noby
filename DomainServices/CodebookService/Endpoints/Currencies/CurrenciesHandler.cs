using DomainServices.CodebookService.Contracts.Endpoints.Currencies;

namespace DomainServices.CodebookService.Endpoints.Currencies
{
    public class CurrenciesHandler
        : IRequestHandler<CurrenciesRequest, List<CurrenciesItem>>
    {
        public async Task<List<CurrenciesItem>> Handle(CurrenciesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.ItemFoundInCache(_cacheKey);
                    return await _cache.GetAllAsync<CurrenciesItem>(_cacheKey);
                }
                else
                {
                    _logger.TryAddItemToCache(_cacheKey);

                    var result = await _connectionProvider.ExecuteDapperRawSqlToList<CurrenciesItem>(_sqlQuery, cancellationToken);
                    await _cache.SetAllAsync(_cacheKey, result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }

        const string _sqlQuery = "SELECT DISTINCT MENA 'Code', POVOLENO_PRO_MENU_PRIJMU 'AllowedForIncomeCurrency', POVOLENO_PRO_MENU_BYDLISTE 'AllowedForResidencyCurrency', DEF 'IsDefault' FROM [SBR].[CIS_STATY] WHERE MENA LIKE '[A-Z][A-Z][A-Z]' ORDER BY MENA ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<CurrenciesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public CurrenciesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<CurrenciesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "Currencies";
    }
}
