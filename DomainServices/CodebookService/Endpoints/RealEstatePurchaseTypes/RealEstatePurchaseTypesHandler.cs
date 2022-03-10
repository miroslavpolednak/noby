using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.RealEstatePurchaseTypes;

namespace DomainServices.CodebookService.Endpoints.RealEstatePurchaseTypes
{
    public class RealEstatePurchaseTypesHandler
        : IRequestHandler<RealEstatePurchaseTypesRequest, List<RealEstatePurchaseTypeItem>>
    {
        public async Task<List<RealEstatePurchaseTypeItem>> Handle(RealEstatePurchaseTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found RealEstatePurchaseTypes in cache");

                    return await _cache.GetAllAsync<RealEstatePurchaseTypeItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading RealEstatePurchaseTypes from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<RealEstatePurchaseTypeItem>("SELECT KOD 'Id', POPIS 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid', DEF 'IsDefault', PORADI 'ORDER' FROM [SBR].[CIS_UCEL_PORIZENI_UV] ORDER BY PORADI ASC")).ToList();

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
        private readonly ILogger<RealEstatePurchaseTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public RealEstatePurchaseTypesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<RealEstatePurchaseTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "RealEstatePurchaseTypes";
    }
}