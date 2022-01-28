using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.ProductLoanKinds;

namespace DomainServices.CodebookService.Endpoints.ProductLoanPurposes;

public class ProductLoanPurposesHandler
    : IRequestHandler<Contracts.Endpoints.ProductLoanPurposes.ProductLoanPurposesRequest, List<Contracts.GenericCodebookItem>>
{
    public async Task<List<Contracts.GenericCodebookItem>> Handle(Contracts.Endpoints.ProductLoanPurposes.ProductLoanPurposesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found ProductLoanPurposes in cache");

                return await _cache.GetAllAsync<Contracts.GenericCodebookItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading ProductLoanPurposes from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<Contracts.GenericCodebookItem>("SELECT ID_UCEL_UVERU 'Id', NAZEV_UCEL_UVERU 'Name', CAST(CASE WHEN DATUM_PLATNOSTI_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsActual' FROM [SBR].[UCEL_UVERU]")).ToList();

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
