using DomainServices.CodebookService.Contracts.Endpoints.Developers;

namespace DomainServices.CodebookService.Endpoints.Developers;

public class DevelopersHandler
    : IRequestHandler<DevelopersRequest, List<DeveloperItem>>
{
    public async Task<List<DeveloperItem>> Handle(DevelopersRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<DeveloperItem>(nameof(DevelopersHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<DeveloperItem>(_sqlQuery, cancellationToken));
    }

    const string _sqlQuery = @"
        SELECT DEVELOPER_ID 'Id', NAZEV 'Name', ICO_RC 'Cin', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid',
        PRIZNAK_OK 'Status', BALICEK_BENEFITU 'BenefitPackage', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_BENEFITU_OD] AND ISNULL([PLATNOST_BENEFITU_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsBenefitValid',
        BENEFITY_NAD_RAMEC_BALICKU 'BenefitsBeyondPackage'
        FROM [SBR].[CIS_DEVELOPER] ORDER BY DEVELOPER_ID ASC
    ";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<DevelopersHandler> _logger;

    public DevelopersHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<DevelopersHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
