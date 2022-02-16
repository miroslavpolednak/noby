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
                _logger.ItemFoundInCache(_cacheKey);

                return await _cache.GetAllAsync<IdentificationDocumentTypesItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<IdentificationDocumentTypesItem>(_sqlQuery, cancellationToken);

                result.ForEach(t => {
                    t.RDMCode = extMapper.FirstOrDefault(s => s.IdentificationDocumentTypeId == t.Id)?.RDMCode;
                });

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

    private class ExtensionMapper
    {
        public int IdentificationDocumentTypeId { get; set; }
        public string? RDMCode { get; set; }
    }

    const string _sqlQuery = "SELECT KOD 'Id', TEXT 'Name', TEXT_SKRATKA 'ShortName', CAST(DEF as bit) 'IsDefault' FROM [SBR].[CIS_TYPY_DOKLADOV] ORDER BY TEXT ASC";
    const string _sqlQueryExtension = "Select * From IdentificationDocumentTypeExtension";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IdentificationDocumentTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public IdentificationDocumentTypesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IdentificationDocumentTypesHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    private const string _cacheKey = "IdentificationDocumentTypes";
}
