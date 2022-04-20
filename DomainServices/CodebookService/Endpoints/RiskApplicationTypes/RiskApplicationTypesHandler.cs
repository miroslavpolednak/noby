using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

namespace DomainServices.CodebookService.Endpoints.RiskApplicationTypes;

public class RiskApplicationTypesHandler
    : IRequestHandler<RiskApplicationTypesRequest, List<RiskApplicationTypeItem>>
{
    public async Task<List<RiskApplicationTypeItem>> Handle(RiskApplicationTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found in cache");

                return await _cache.GetAllAsync<RiskApplicationTypeItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<RiskApplicationTypeItem>(_sqlQuery)).ToList();
                    result.ForEach(t =>
                    {
                        if (!string.IsNullOrEmpty(t.MA))
                            t.MarketingActions = t.MA.Split(",").Select(t => Convert.ToInt32(t)).ToList();
                        if (!string.IsNullOrEmpty(t.ProductId))
                            t.ProductTypeId = t.ProductId.Split(",").Select(t => Convert.ToInt32(t)).ToList();
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

    const string _sqlQuery = @"SELECT ID 'Id', MANDANT 'Mandant', UV_PRODUKT_ID 'ProductId', MA, DRUH_UVERU 'LoanKindId', CAST(LTV_OD as int) 'LtvFrom', CAST(LTV_DO as int) 'LtvTo', CLUSTER_CODE 'C4mAplCode', C4M_APL_TYPE_ID 'C4mAplTypeId', C4M_APL_TYPE_NAZEV 'Name', CASE WHEN SYSDATETIME() BETWEEN [DATUM_OD] AND ISNULL([DATUM_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
FROM [SBR].CIS_APL_TYPE ORDER BY ID ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<RiskApplicationTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public RiskApplicationTypesHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<RiskApplicationTypesHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "RiskApplicationTypes";
}
