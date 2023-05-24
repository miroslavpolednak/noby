using CIS.Core.ErrorCodes;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class SbWebApiCommonDataProvider
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly IMediator _mediator;

    public SbWebApiCommonDataProvider(ICodebookServiceClient codebookService, IMediator mediator)
    {
        _codebookService = codebookService;
        _mediator = mediator;
    }

    public async Task<GetTaskListResponse> GetAndUpdateTasksList(long caseId, Func<IList<int>, Task<IList<IReadOnlyDictionary<string, string>>>> loadData, CancellationToken cancellationToken)
    {
        // load codebooks
        var taskStateIds = await GetValidTaskStateIds(cancellationToken);

        var tasks = (await loadData(taskStateIds))
            .Select(taskData => taskData.ToWorkflowTask())
            .ToList();

        // update active tasks
        var updateRequest = new UpdateActiveTasksRequest
        {
            CaseId = caseId
        };
        updateRequest.Tasks.AddRange(tasks.Select(i => i.ToUpdateTaskItem()));
        await _mediator.Send(updateRequest, cancellationToken);

        // response
        var response = new GetTaskListResponse();
        response.Tasks.AddRange(tasks);
        return response;
    }

    public async Task<IList<int>> GetValidTaskStateIds(CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStates(cancellationToken);

        return taskStates.Where(i => !i.Flag.HasFlag(CodebookService.Contracts.v1.WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.Inactive)).Select(i => i.Id).ToList();
    }
}