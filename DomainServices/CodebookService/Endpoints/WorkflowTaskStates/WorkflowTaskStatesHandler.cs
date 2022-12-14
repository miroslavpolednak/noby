using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskStates;

public class WorkflowTaskStatesHandler
    : IRequestHandler<WorkflowTaskStatesRequest, List<WorkflowTaskStateItem>>
{
    public async Task<List<WorkflowTaskStateItem>> Handle(WorkflowTaskStatesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<WorkflowTaskStateItem>(nameof(WorkflowTaskStatesHandler), async () =>
        {
            var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

            var result = await _connectionProvider.ExecuteDapperRawSqlToList<WorkflowTaskStateItem>(_sqlQuery, cancellationToken);

            var flags = extMapper.ToDictionary(i => i.WorkflowTaskStateId, i => i.Flag);

            result.ForEach(i =>
            {
                i.Flag = flags.ContainsKey(i.Id) ? flags[i.Id] : EWorkflowTaskStateFlag.None;
            });

            return result;
        });
    }


    private class ExtensionMapper
    {
        public int WorkflowTaskStateId { get; set; }
        public EWorkflowTaskStateFlag Flag { get; set; }
    }


    const string _sqlQuery = @"SELECT KOD 'Id', TEXT 'Name' FROM [SBR].[CIS_WFL_UKOLY_STAVY] ORDER BY KOD ASC";
    const string _sqlQueryExtension = "SELECT WorkflowTaskStateId, Flag From WorkflowTaskStateExtension";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<WorkflowTaskStatesHandler> _logger;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;


    public WorkflowTaskStatesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<WorkflowTaskStatesHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }
}