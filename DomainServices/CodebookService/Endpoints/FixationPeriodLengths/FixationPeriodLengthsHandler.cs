using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.FixationPeriodLengths;

namespace DomainServices.CodebookService.Endpoints.FixationPeriodLengths;

public class FixationPeriodLengthsHandler
    : IRequestHandler<FixationPeriodLengthsRequest, List<FixationPeriodLengthsItem>>
{
    public async Task<List<FixationPeriodLengthsItem>> Handle(FixationPeriodLengthsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.LogDebug("Found FixationPeriodLengths in cache");

                return await _cache.GetAllAsync<FixationPeriodLengthsItem>(_cacheKey);
            }
            else
            {
                _logger.LogDebug("Reading FixationPeriodLengths from database");

                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var result = (await connection.QueryAsync<FixationPeriodLengthsItem>("SELECT PERIODA_FIXACE 'FixationMonths', 1 'ProductInstanceTypeId' FROM [SBR].[CIS_PERIODY_FIXACE] WHERE PLATNOST_OD<GETDATE() AND PLATNOST_DO IS NULL")).ToList();

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
    private readonly ILogger<FixationPeriodLengthsHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;

    public FixationPeriodLengthsHandler(
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<FixationPeriodLengthsHandler> logger)
    {
        _cache = cache;
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    private const string _cacheKey = "FixationPeriodLengths";
}
