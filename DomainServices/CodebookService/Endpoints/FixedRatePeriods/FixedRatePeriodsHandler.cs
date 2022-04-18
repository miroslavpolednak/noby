using DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods;

namespace DomainServices.CodebookService.Endpoints.FixedRatePeriods;

public class FixedRatePeriodsHandler
    : IRequestHandler<FixedRatePeriodsRequest, List<FixedRatePeriodsItem>>
{
    public async Task<List<FixedRatePeriodsItem>> Handle(FixedRatePeriodsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<FixedRatePeriodsItem>(nameof(FixedRatePeriodsHandler), async () =>
                    await _connectionProvider.ExecuteDapperRawSqlToList<FixedRatePeriodsItem>(_sqlQuery, cancellationToken)
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT PERIODA_FIXACE 'FixedRatePeriod', 20001 'ProductTypeId' FROM SBR.CIS_PERIODY_FIXACE WHERE PLATNOST_OD<GETDATE() AND PLATNOST_DO IS NULL";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<FixedRatePeriodsHandler> _logger;

    public FixedRatePeriodsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<FixedRatePeriodsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
