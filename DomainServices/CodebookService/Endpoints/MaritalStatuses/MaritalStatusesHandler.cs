using DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses;

namespace DomainServices.CodebookService.Endpoints.MaritalStatuses;

public class MaritalStatusesHandler
    : IRequestHandler<MaritalStatusesRequest, List<MaritalStatusItem>>
{
    public async Task<List<MaritalStatusItem>> Handle(MaritalStatusesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.ItemFoundInCache(_cacheKey);

                return await _cache.GetAllAsync<MaritalStatusItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<MaritalStatusItem>(_sqlQuery, cancellationToken);

                result.ForEach(t => {
                    t.RdmMaritalStatusCode = extMapper.FirstOrDefault(s => s.MaritalStatusId == t.Id)?.RDMCode;
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
        public int MaritalStatusId { get; set; }
        public string? RDMCode { get; set; }
    }

    const string _sqlQuery = "SELECT KOD As ID, [TEXT] As [Name], Cast(1 As bit) As IsValid FROM [xxd0vss].[SBR].[CIS_RODINNE_STAVY] Order By [TEXT]";
    const string _sqlQueryExtension = "Select * From MaritalStatusExtension";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<MaritalStatusesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public MaritalStatusesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<MaritalStatusesHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    private const string _cacheKey = "MaritalStatuses";
}