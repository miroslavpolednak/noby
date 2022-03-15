using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.AcademicDegreesAfter;

namespace DomainServices.CodebookService.Endpoints.AcademicDegreesAfter
{
    public class AcademicDegreesAfterHandler
        : IRequestHandler<AcademicDegreesAfterRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(AcademicDegreesAfterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found AcademicDegreesAfter in cache");

                    return await _cache.GetAllAsync<GenericCodebookItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading AcademicDegreesAfter from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<GenericCodebookItem>("SELECT KOD 'Id', TEXT 'Name' FROM [SBR].[CIS_TITULY_ZA] WHERE KOD>0 ORDER BY TEXT ASC")).ToList();
                        
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
        private readonly ILogger<AcademicDegreesAfterHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public AcademicDegreesAfterHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<AcademicDegreesAfterHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "AcademicDegreesAfter";
    }
}
