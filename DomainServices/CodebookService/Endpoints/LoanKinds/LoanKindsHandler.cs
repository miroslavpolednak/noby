using DomainServices.CodebookService.Contracts.Endpoints.LoanKinds;

namespace DomainServices.CodebookService.Endpoints.LoanKinds;

public class LoanKindsHandler
    : IRequestHandler<LoanKindsRequest, List<LoanKindsItem>>
{
    public async Task<List<LoanKindsItem>> Handle(LoanKindsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.ItemFoundInCache(_cacheKey);
                return await _cache.GetAllAsync<LoanKindsItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<LoanKindsItem>(_sqlQuery, cancellationToken);
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

    const string _sqlQuery = @"SELECT KOD 'Id', DRUH_UVERU_TEXT 'Name', CAST(DEFAULT_HODNOTA as bit) 'IsDefault', CAST(CASE WHEN DATUM_DO_ES IS NULL THEN 1 ELSE 0 END as bit) 'IsValid' FROM [SBR].[CIS_DRUH_UVERU]";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<LoanKindsHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public LoanKindsHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<LoanKindsHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "LoanKinds";
}
