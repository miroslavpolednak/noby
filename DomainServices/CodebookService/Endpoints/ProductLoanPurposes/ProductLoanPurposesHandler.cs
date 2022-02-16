using DomainServices.CodebookService.Contracts.Endpoints.ProductLoanPurposes;

namespace DomainServices.CodebookService.Endpoints.ProductLoanPurposes;

public class ProductLoanPurposesHandler
    : IRequestHandler<ProductLoanPurposesRequest, List<ProductLoanPurposesItem>>
{
    public async Task<List<ProductLoanPurposesItem>> Handle(ProductLoanPurposesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.ItemFoundInCache(_cacheKey);
                return await _cache.GetAllAsync<ProductLoanPurposesItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<ProductLoanPurposesItem>(_sqlQuery, cancellationToken);
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

    const string _sqlQuery = @"
SELECT KOD 'Id', TEXT 'Name', 2 'Mandant', CAST(CASE WHEN DATUM_PLATNOSTI_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsValid' 
FROM SBR.CIS_UCEL_UVERU_INT1
";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ProductLoanPurposesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public ProductLoanPurposesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<ProductLoanPurposesHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "ProductLoanPurposes";
}
