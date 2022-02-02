using Dapper;
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
                _logger.LogDebug("Found ProductLoanPurposes in cache");

                return await _cache.GetAllAsync<ProductLoanPurposesItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading ProductLoanPurposes from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<ProductLoanPurposesItem>(_sqlQuery)).ToList();

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

    const string _sqlQuery = @"
SELECT KOD 'Id', TEXT 'Name', 1 'Mandant', CAST(CASE WHEN DATUM_PLATNOSTI_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsValid' 
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
