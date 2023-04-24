using DomainServices.CodebookService.Contracts.Endpoints.WorkflowConsultationMatrix;

namespace DomainServices.CodebookService.Endpoints.WorkflowConsultationMatrix;

public class WorkflowConsultationMatrixHandler
    : IRequestHandler<WorkflowConsultationMatrixRequest, List<WorkflowConsultationMatrixItem>>
{
    public async Task<List<WorkflowConsultationMatrixItem>> Handle(WorkflowConsultationMatrixRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<WorkflowConsultationMatrixItem>(nameof(WorkflowConsultationMatrixHandler), async () =>
        {
            var xxdResult = await _connectionProvider.ExecuteDapperRawSqlToList<(int Kod, string Text)>(_sqlXxd, cancellationToken);
            var matrix = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<(int TaskSubtypeId, int ProcessTypeId, int ProcessPhaseId, bool IsConsultation)>(_sqlCodebook, cancellationToken);

            return xxdResult.Select(t => new WorkflowConsultationMatrixItem
            {
                TaskSubtypeId = t.Kod,
                TaskSubtypeName = t.Text,
                IsValidFor = matrix
                    .Where(x => x.TaskSubtypeId == t.Kod)
                    .Select(x => new WorkflowConsultationMatrixItemValidity
                    {
                        ProcessPhaseId = x.ProcessPhaseId,
                        ProcessTypeId = x.ProcessTypeId
                    }).ToList()
            })
            .ToList();
        });
    }

    const string _sqlXxd = "SELECT CAST(KOD as int) 'Kod', [TEXT] 'Text' FROM SBR.v_htedm_cis_wfl_cis_hodnoty WHERE ciselnik_id = 139";
    const string _sqlCodebook = "SELECT [TaskSubtypeId],[ProcessTypeId],[ProcessPhaseId],[IsConsultation] FROM [dbo].[WorkflowConsultationMatrix]";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public WorkflowConsultationMatrixHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }
}
