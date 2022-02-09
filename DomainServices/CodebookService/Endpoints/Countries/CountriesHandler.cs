using Dapper;
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
                    _logger.LogDebug("Found Countries in cache");

                    return await _cache.GetAllAsync<CountriesItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading Countries from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<CountriesItem>(_sql)).ToList();

                        await _cache.SetAllAsync(_cacheKey, result);

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        const string _sql = "SELECT KOD 'Id', TEXT 'Name', TEXT_CELY 'FullName', MENA 'CurrencyCode', CAST(POVOLENO_PRO_MENU_PRIJMU as bit) 'IsAllowedForIncomeChange', CAST(POVOLENO_PRO_MENU_BYDLISTE as bit) 'IsAllowedForResidenceChange' FROM SBR.CIS_STATY ORDER BY TEXT ASC";

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
