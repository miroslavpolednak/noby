using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace DomainServices.CodebookService.Endpoints.ProductTypes;

public class ProductTypesHandler
    : IRequestHandler<ProductTypesRequest, List<ProductTypeItem>>
{
    public async Task<List<ProductTypeItem>> Handle(ProductTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found ProductTypeItem in cache");

                return await _cache.GetAllAsync<ProductTypeItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading ProductTypeItem from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<ProductTypeItem>(_sql)).ToList();

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

    const string _sql = @"
SELECT KOD_PRODUKTU 'Id', NAZOV_PRODUKTU 'Name', null 'Description', MANDANT 'Mandant', null 'ProductCategory', PORADIE_ZOBRAZENIA 'Order', MIN_VYSKA_UV 'LoanAmountMin', MAX_VYSKA_UV 'LoanAmountMax', MIN_SPLATNOST_V_ROKOCH 'LoanDurationMin', MAX_SPLATNOST_V_ROKOCH 'LoanDurationMax', MIN_VYSKA_LTV 'LtvMin', MAX_VYSKA_LTV 'LtvMax', 'xxx' 'MpHomeApiLoanType', CAST(CASE WHEN PLATNOST_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' 
FROM SBR.CIS_PRODUKTY_UV
ORDER BY PORADIE_ZOBRAZENIA ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ProductTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public ProductTypesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<ProductTypesHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "ProductTypes";
}
