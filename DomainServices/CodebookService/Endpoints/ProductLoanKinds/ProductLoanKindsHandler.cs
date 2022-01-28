using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.ProductLoanKinds;

namespace DomainServices.CodebookService.Endpoints.ProductLoanKinds;

public class ProductLoanKindsHandler
    : IRequestHandler<ProductLoanKindsRequest, List<ProductLoanKindsItem>>
{
    public async Task<List<ProductLoanKindsItem>> Handle(ProductLoanKindsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found ProductLoanKinds in cache");

                return await _cache.GetAllAsync<ProductLoanKindsItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading ProductLoanKinds from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<ProductLoanKindsItem>("SELECT ID_DRUH_UVERU 'Id', NAZEV_DRUH_UVERU 'Name', 1 'ProductInstanceTypeId', CAST(CASE WHEN DATUM_DO_ES IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' FROM [SBR].[DRUH_UVERU]")).ToList();

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
    private readonly ILogger<ProductLoanKindsHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public ProductLoanKindsHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<ProductLoanKindsHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "ProductLoanKinds";
}
