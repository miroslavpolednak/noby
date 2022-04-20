using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;

namespace DomainServices.CodebookService.Endpoints.ObligationTypes
{
    public class ObligationTypesHandler
        : IRequestHandler<ObligationTypesRequest, List<GenericCodebookItemWithCode>>
    {
        public async Task<List<GenericCodebookItemWithCode>> Handle(ObligationTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found ObligationTypes in cache");

                    return await _cache.GetAllAsync<GenericCodebookItemWithCode>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading ObligationTypes from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<GenericCodebookItemWithCode>(_sqlQuery)).ToList();

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

        const string _sqlQuery = "SELECT KOD 'Id', CODE 'Code', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].CIS_DRUH_ZAVAZKU ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ObligationTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public ObligationTypesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ObligationTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "ObligationTypes";
    }
}