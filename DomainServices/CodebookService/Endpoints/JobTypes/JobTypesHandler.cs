using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.JobTypes;

namespace DomainServices.CodebookService.Endpoints.JobTypes
{
    public class JobTypesHandler
        : IRequestHandler<JobTypesRequest, List<JobTypeItem>>
    {
        public async Task<List<JobTypeItem>> Handle(JobTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found JobTypes in cache");

                    return await _cache.GetAllAsync<JobTypeItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading JobTypes from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<JobTypeItem>("SELECT KOD 'Id', TEXT 'Name', DEF 'IsDefault' FROM [SBR].[CIS_PRACOVNI_POZICE] ORDER BY KOD ASC")).ToList();

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
        private readonly ILogger<JobTypesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public JobTypesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<JobTypesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "JobTypes";
    }
}