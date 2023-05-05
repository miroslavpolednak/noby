using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskConsultationTypes;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskConsultationTypes;

public class WorkflowTaskConsultationTypesHandler
    : IRequestHandler<WorkflowTaskConsultationTypesRequest, List<GenericCodebookItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<WorkflowTaskConsultationTypesHandler> _logger;

    public WorkflowTaskConsultationTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<WorkflowTaskConsultationTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    #endregion

    private const string _sqlQuery =
            "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN DATUM_OD AND ISNULL(DATUM_DO, '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM SBR.HTEDM_CIS_WFL_CISELNIKY_HODNOTY WHERE CISELNIK_ID = 139 ORDER BY KOD";

    public async Task<List<GenericCodebookItem>> Handle(WorkflowTaskConsultationTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(WorkflowTaskConsultationTypesHandler), async () => await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken));
    }

}