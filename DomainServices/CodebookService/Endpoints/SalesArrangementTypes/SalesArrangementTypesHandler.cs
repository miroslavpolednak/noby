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
                    _logger.ItemFoundInCache(_cacheKey);

                    return await _cache.GetAllAsync<SalesArrangementTypeItem>(_cacheKey);
                }
                else
                {
                    _logger.TryAddItemToCache(_cacheKey);

                    var result = await _connectionProvider.ExecuteDapperRawSqlToList<SalesArrangementTypeItem>(_sqlQuery, cancellationToken);
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

        private const string _sqlQuery =
            "SELECT Id, Name, ProductTypeId, IsDefault FROM [dbo].[SalesArrangementType] ORDER BY Name ASC";

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

        const string _cacheKey = "SalesArrangementTypes";
    }
}
