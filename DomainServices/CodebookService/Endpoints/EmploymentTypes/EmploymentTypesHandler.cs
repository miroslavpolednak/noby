using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.EmploymentTypes;

namespace DomainServices.CodebookService.Endpoints.EmploymentTypes;

public class EmploymentTypesHandler
    : IRequestHandler<EmploymentTypesRequest, List<GenericCodebookItemWithCode>>
{
    public async Task<List<GenericCodebookItemWithCode>> Handle(EmploymentTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GenericCodebookItemWithCode>(nameof(EmploymentTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItemWithCode>(_sqlQuery, cancellationToken));
    }

    private const string _sqlQuery =
            "SELECT Kod 'Id', CODE 'Code', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO, '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.CIS_PRACOVNY_POMER ORDER BY Kod";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<EmploymentTypesHandler> _logger;

    public EmploymentTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<EmploymentTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
