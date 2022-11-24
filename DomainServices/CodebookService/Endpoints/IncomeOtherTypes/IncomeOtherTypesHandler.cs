using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeOtherTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeOtherTypes;

public class IncomeOtherTypesHandler
    : IRequestHandler<IncomeOtherTypesRequest, List<GenericCodebookItemWithCode>>
{
    public async Task<List<GenericCodebookItemWithCode>> Handle(IncomeOtherTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GenericCodebookItemWithCode>(nameof(IncomeOtherTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItemWithCode>(_sqlQuery, cancellationToken));
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT_CZE 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_ZDROJ_PRIJMU_VEDLAJSI] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IncomeOtherTypesHandler> _logger;

    public IncomeOtherTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IncomeOtherTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
