using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementTypes;

namespace DomainServices.CodebookService.Endpoints.SalesArrangementTypes
{
    public class SalesArrangementTypesHandler
        : IRequestHandler<SalesArrangementTypesRequest, List<SalesArrangementTypeItem>>
    {
        public async Task<List<SalesArrangementTypeItem>> Handle(SalesArrangementTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found SalesArrangementTypes in cache");

                    return await _cache.GetAllAsync<SalesArrangementTypeItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading SalesArrangementTypes from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<SalesArrangementTypeItem>("SELECT Id, Name FROM [dbo].[SalesArrangementType] ORDER BY Name ASC")).ToList();

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

        private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;
        private readonly ILogger<SalesArrangementTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public SalesArrangementTypesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider connectionProvider,
            ILogger<SalesArrangementTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "SalesArrangementTypes";
    }
}
