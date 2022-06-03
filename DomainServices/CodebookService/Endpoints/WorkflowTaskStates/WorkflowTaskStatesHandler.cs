using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;

namespace DomainServices.CodebookService.Endpoints.WorkflowTaskStates;

public class WorkflowTaskStatesHandler
    : IRequestHandler<WorkflowTaskStatesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(WorkflowTaskStatesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(WorkflowTaskStatesHandler), async () =>
                await _connectionProvider.ExecuteDapperRawSqlToList<GenericCodebookItem>(_sqlQuery, cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', TEXT 'Name', 1 'IsValid' FROM [SBR].[CIS_WFL_UKOLY_STAVY] ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<WorkflowTaskStatesHandler> _logger;

    public WorkflowTaskStatesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<WorkflowTaskStatesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}