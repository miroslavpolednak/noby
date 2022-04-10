using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.ClassficationOfEconomicActivities;

namespace DomainServices.CodebookService.Endpoints.RealEstateTypes
{
    public class ClassficationOfEconomicActivitiesHandler
        : IRequestHandler<ClassficationOfEconomicActivitiesRequest, List<ClassficationOfEconomicActivitiesItem>>
    {
        public async Task<List<ClassficationOfEconomicActivitiesItem>> Handle(ClassficationOfEconomicActivitiesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found RealEstateTypes in cache");

                    return await _cache.GetAllAsync<ClassficationOfEconomicActivitiesItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading RealEstateTypes from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<ClassficationOfEconomicActivitiesItem>("SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_OKEC] ORDER BY KOD ASC")).ToList();

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
        private readonly ILogger<RealEstateTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public ClassficationOfEconomicActivitiesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<RealEstateTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "ClassficationOfEconomicActivities";
    }
}