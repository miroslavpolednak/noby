using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeMainTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeMainTypes;

public class IncomeMainTypesHandler
    : IRequestHandler<IncomeMainTypesRequest, List<GenericCodebookItemWithCode>>
{
    public async Task<List<GenericCodebookItemWithCode>> Handle(IncomeMainTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate(nameof(IncomeMainTypesHandler), async () =>
            await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItemWithCode>(_sqlQuery, cancellationToken)
        );
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_ZDROJ_PRIJMU_HLAVNI] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IncomeMainTypesHandler> _logger;

    public IncomeMainTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IncomeMainTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
