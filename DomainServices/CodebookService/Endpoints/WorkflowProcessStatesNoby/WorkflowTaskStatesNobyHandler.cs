using DomainServices.CodebookService.Contracts.Endpoints.WorkflowProcessStatesNoby;

namespace DomainServices.CodebookService.Endpoints.WorkflowProcessStatesNoby;
internal class WorkflowProcessStatesNobyHandler
    : IRequestHandler<WorkflowProcessStatesNobyRequest, List<WorkflowProcessStateNobyItem>>
{
    #region Construction

    const string _sqlQuery = @"SELECT [Id],[Name],[Indicator] FROM [dbo].[WorkflowProcessStatesNoby] ORDER BY [Id] ASC";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public WorkflowProcessStatesNobyHandler(CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion

    public async Task<List<WorkflowProcessStateNobyItem>> Handle(WorkflowProcessStatesNobyRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<WorkflowProcessStateNobyItem>(nameof(WorkflowProcessStatesNobyHandler), async () => await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<WorkflowProcessStateNobyItem>(_sqlQuery, cancellationToken));
    }
}
