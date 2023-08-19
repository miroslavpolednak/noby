using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class SbWebApiCommonDataProvider
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly Services.ActiveTasksService _activeTasks;

    public SbWebApiCommonDataProvider(ICodebookServiceClient codebookService, Services.ActiveTasksService activeTasks)
    {
        _codebookService = codebookService;
        _activeTasks = activeTasks;
    }

    public async Task<GetTaskListResponse> GetAndUpdateTasksList(long caseId, Func<IList<int>, Task<IList<IReadOnlyDictionary<string, string>>>> loadData, CancellationToken cancellationToken)
    {
        // load codebooks
        var taskStateIds = await GetValidTaskStateIds(cancellationToken);

        var tasks = (await loadData(taskStateIds))
            .Select(taskData => taskData.ToWorkflowTask())
            .ToList();

        // update active tasks
        await _activeTasks.SyncActiveTasks(caseId, tasks, cancellationToken);

        // response
        var response = new GetTaskListResponse();
        response.Tasks.AddRange(tasks);
        return response;
    }

    public async Task<IList<int>> GetValidTaskStateIds(CancellationToken cancellationToken)
    {
        return (await _codebookService.WorkflowTaskStates(cancellationToken))
            .Select(i => i.Id)
            .ToList();
    }
}