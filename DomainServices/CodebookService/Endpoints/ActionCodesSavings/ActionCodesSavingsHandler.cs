using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.ActionCodesSavingsLoan;

namespace DomainServices.CodebookService.Endpoints.ActionCodesSavingsLoan
{
    public class ActionCodesSavingsLoanHandler
        : IRequestHandler<ActionCodesSavingsLoanRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(ActionCodesSavingsLoanRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found ActionCodesSavingsLoan in cache");

                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading ActionCodesSavingsLoan from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<GenericCodebookItem>("SELECT ID_AKCE_UV 'Id', NAZEV_AKCE_UV 'Name', CAST(CASE WHEN PLATNOST_DO_ES IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' FROM [SBR].[AKCE_UV] ORDER BY NAZEV_AKCE_UV ASC")).ToList();
                        
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

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ActionCodesSavingsLoanHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public ActionCodesSavingsLoanHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ActionCodesSavingsLoanHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "ActionCodesSavingsLoan";
    }
}
