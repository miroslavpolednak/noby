using DomainServices.CodebookService.Contracts.Endpoints.ObligationLaExposures;

namespace DomainServices.CodebookService.Endpoints.ObligationLaExposures;

public class ObligationLaExposuresHandler
    : IRequestHandler<ObligationLaExposuresRequest, List<ObligationLaExposureItem>>
{
    public async Task<List<ObligationLaExposureItem>> Handle(ObligationLaExposuresRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ObligationLaExposureItem>(nameof(ObligationLaExposuresHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<ObligationLaExposureItem>(_sqlQuery, cancellationToken));
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'RdmCode', TEXT 'Name', DRUH_ZAVAZKU_KATEGORIE 'ObligationTypeId', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].CIS_ZAVAZKY_LA_EXPOSURE ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ObligationLaExposuresHandler> _logger;

    public ObligationLaExposuresHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<ObligationLaExposuresHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
