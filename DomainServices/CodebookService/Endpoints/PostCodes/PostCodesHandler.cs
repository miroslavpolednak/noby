using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.PostCodes;

namespace DomainServices.CodebookService.Endpoints.PostCodes
{
    public class PostCodesHandler
        : IRequestHandler<PostCodesRequest, List<PostCodeItem>>
    {
        public async Task<List<PostCodeItem>> Handle(PostCodesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_cache.Exists(_cacheKey))
                {
                    _logger.LogDebug("Found PostCodes in cache");

                    return await _cache.GetAllAsync<PostCodeItem>(_cacheKey);
                }
                else
                {
                    _logger.LogDebug("Reading PostCodes from database");

                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        var result = (await connection.QueryAsync<PostCodeItem>("SELECT TOP 20 PSC 'PostCode', NAZEV 'Name', KOD_KRAJA 'Disctrict', KOD_OBCE 'Municipality' FROM [SBR].[CIS_PSC] ORDER BY PSC ASC")).ToList();

                        result.ForEach(i => {
                            i.Name = i.Name.Trim();
                        });

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
        private readonly ILogger<PostCodesHandler> _logger;
        private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

        public PostCodesHandler(
            CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<PostCodesHandler> logger)
        {
            _cache = cache;
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        private const string _cacheKey = "PostCodes";
    }
}