using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

namespace DomainServices.CodebookService.Endpoints.EducationLevels
{
    public class EducationLevelsHandler
        : IRequestHandler<EducationLevelsRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(EducationLevelsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found EducationLevels in cache");

                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading EducationLevels from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<GenericCodebookItem>("SELECT ID_VZDELANI 'Id', NAZEV_VZDELANI 'Name' FROM [SBR].[CIS_VZDELANI] ORDER BY ID_VZDELANI ASC")).ToList();

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
        private readonly ILogger<EducationLevelsHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public EducationLevelsHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<EducationLevelsHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "EducationLevels";
    }
}
