using DomainServices.CodebookService.Contracts.Endpoints.Countries;

namespace DomainServices.CodebookService.Endpoints.Countries
{
    public class CountriesHandler
        : IRequestHandler<CountriesRequest, List<CountriesItem>>
    {
        public async Task<List<CountriesItem>> Handle(CountriesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.ItemFoundInCache(_cacheKey);
                    return await _cache.GetAllAsync<CountriesItem>(_cacheKey);
                }
                else
                {
                    _logger.TryAddItemToCache(_cacheKey);

                    var result = await _connectionProvider.ExecuteDapperRawSqlToList<CountriesItem>(_sqlQuery, cancellationToken);
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

        const string _sqlQuery = "SELECT KOD 'Id', TEXT 'Name', TEXT_CELY 'FullName', MENA 'CurrencyCode', CAST(POVOLENO_PRO_MENU_PRIJMU as bit) 'IsAllowedForIncomeChange', CAST(POVOLENO_PRO_MENU_BYDLISTE as bit) 'IsAllowedForResidenceChange' FROM SBR.CIS_STATY ORDER BY TEXT ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<CountriesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public CountriesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<CountriesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "Countries";
    }
}
