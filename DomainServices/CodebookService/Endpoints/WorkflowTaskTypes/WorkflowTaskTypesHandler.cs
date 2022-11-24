using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskTypes;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskTypes;

public class WorkflowTaskTypesHandler
    : IRequestHandler<WorkflowTaskTypesRequest, List<WorkflowTaskTypeItem>>
{
    public async Task<List<WorkflowTaskTypeItem>> Handle(WorkflowTaskTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<WorkflowTaskTypeItem>(nameof(WorkflowTaskTypesHandler), async () =>
        {
            var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

            var dictCategoriesById = extMapper.ToDictionary(i => i.WorkflowTaskTypeId, i=>i.CategoryId);

            var result = await _connectionProvider.ExecuteDapperRawSqlToList<WorkflowTaskTypeItem>(_sqlQuery, cancellationToken);

            result.ForEach(t => {
                t.CategoryId = dictCategoriesById.ContainsKey(t.Id) ? dictCategoriesById[t.Id] : null;
            });

            return result;
        });
    }

    private class ExtensionMapper
    {
        public int WorkflowTaskTypeId { get; set; }
        public int CategoryId { get; set; }
    }

    const string _sqlQuery = @"SELECT UKOL_TYP 'Id', UKOL_NAZOV 'Name' FROM [SBR].[CIS_WFL_UKOLY] ORDER BY UKOL_TYP ASC";
    const string _sqlQueryExtension = "SELECT [WorkflowTaskTypeId], [CategoryId] FROM WorkflowTaskTypeExtension";
    
    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<WorkflowTaskTypesHandler> _logger;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public WorkflowTaskTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<WorkflowTaskTypesHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }
}