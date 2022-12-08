using DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods;

namespace DomainServices.CodebookService.Endpoints.FixedRatePeriods;

public class FixedRatePeriodsHandler
    : IRequestHandler<FixedRatePeriodsRequest, List<FixedRatePeriodsItem>>
{
    public async Task<List<FixedRatePeriodsItem>> Handle(FixedRatePeriodsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<FixedRatePeriodsItem>(nameof(FixedRatePeriodsHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<FixedRatePeriodsItem>(_sqlQuery, cancellationToken)
            );
    }

    const string _sqlQuery = @"SELECT KOD_PRODUKTU 'ProductTypeId', PERIODA_FIXACE 'FixedRatePeriod', NULLIF(MANDANT, 0) 'MandantId', NOVY_PRODUKT 'IsNewProduct', ALGORITMUS_SAZBY 'InterestRateAlgorithm', 
                                CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.CIS_PERIODY_FIXACE_V";

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