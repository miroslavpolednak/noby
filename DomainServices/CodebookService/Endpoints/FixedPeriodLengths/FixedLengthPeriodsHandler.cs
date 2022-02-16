using DomainServices.CodebookService.Contracts.Endpoints.FixedPeriodLengths;

namespace DomainServices.CodebookService.Endpoints.FixedPeriodLengths;

public class FixedLengthPeriodsHandler
    : IRequestHandler<FixedLengthPeriodsRequest, List<FixedLengthPeriodsItem>>
{
    public async Task<List<FixedLengthPeriodsItem>> Handle(FixedLengthPeriodsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.ItemFoundInCache(_cacheKey);
                return await _cache.GetAllAsync<FixedLengthPeriodsItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<FixedLengthPeriodsItem>(_sqlQuery, cancellationToken);
                await _cache.SetAllAsync(_cacheKey, result);
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    const string _sqlQuery = @"
SELECT PERIODA_FIXACE 'FixedLengthPeriod', 20001 'ProductTypeId' 
FROM SBR.CIS_PERIODY_FIXACE 
WHERE PLATNOST_OD<GETDATE() AND PLATNOST_DO IS NULL";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<FixedLengthPeriodsHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public FixedLengthPeriodsHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<FixedLengthPeriodsHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "FixationPeriodLengths";
}
