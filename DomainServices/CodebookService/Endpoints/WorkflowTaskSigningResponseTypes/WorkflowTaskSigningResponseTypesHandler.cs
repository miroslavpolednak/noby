using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskSigningResponseTypes;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskSigningResponseTypes;

public class WorkflowTaskSigningResponseTypesHandler
    : IRequestHandler<WorkflowTaskSigningResponseTypesRequest, List<GenericCodebookItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdHfDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<WorkflowTaskSigningResponseTypesHandler> _logger;

    public WorkflowTaskSigningResponseTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdHfDapperConnectionProvider> connectionProvider, 
        ILogger<WorkflowTaskSigningResponseTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    #endregion

    private const string _sqlQuery =
            "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN DATUM_OD AND ISNULL(DATUM_DO, '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID = 144 ORDER BY KOD";

    public async Task<List<GenericCodebookItem>> Handle(WorkflowTaskSigningResponseTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(WorkflowTaskSigningResponseTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken));
    }

}