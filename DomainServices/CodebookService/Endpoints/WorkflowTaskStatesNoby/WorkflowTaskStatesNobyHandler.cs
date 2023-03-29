using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStatesNoby;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskStatesNoby;
internal class WorkflowTaskStatesNobyHandler
    : IRequestHandler<WorkflowTaskStatesNobyRequest, List<WorkflowTaskStateNobyItem>>
{
    #region Construction

    const string _sqlQuery = @"SELECT [Id],[Name],[Filter],[Indicator] FROM [dbo].[WorkflowTaskStatesNoby] ORDER BY [Id] ASC";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public WorkflowTaskStatesNobyHandler(CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion

    public async Task<List<WorkflowTaskStateNobyItem>> Handle(WorkflowTaskStatesNobyRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<WorkflowTaskStateNobyItem>(nameof(WorkflowTaskStatesNobyHandler), async () => await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<WorkflowTaskStateNobyItem>(_sqlQuery, cancellationToken));
    }
}
