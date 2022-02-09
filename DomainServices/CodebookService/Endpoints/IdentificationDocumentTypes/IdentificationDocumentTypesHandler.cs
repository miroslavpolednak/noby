using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes;

namespace DomainServices.CodebookService.Endpoints.IdentificationDocumentTypes;

public class IdentificationDocumentTypesHandler
    : IRequestHandler<IdentificationDocumentTypesRequest, List<IdentificationDocumentTypesItem>>
{
    public async Task<List<IdentificationDocumentTypesItem>> Handle(IdentificationDocumentTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found IdentificationDocumentTypes in cache");

                return await _cache.GetAllAsync<IdentificationDocumentTypesItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading IdentificationDocumentTypes from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<IdentificationDocumentTypesItem>(_sql)).ToList();
                        
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

    const string _sql = "SELECT KOD 'Id', TEXT 'Name', TEXT_SKRATKA 'ShortName', CAST(DEF as bit) 'IsDefault' FROM [SBR].[CIS_TYPY_DOKLADOV] ORDER BY TEXT ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IdentificationDocumentTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public IdentificationDocumentTypesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IdentificationDocumentTypesHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "IdentificationDocumentTypes";
}
