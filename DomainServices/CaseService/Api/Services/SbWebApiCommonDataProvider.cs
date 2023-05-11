using CIS.Core.ErrorCodes;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class SbWebApiCommonDataProvider
{
    private readonly ICodebookServiceClients _codebookService;
    private readonly IMediator _mediator;

    public SbWebApiCommonDataProvider(ICodebookServiceClients codebookService, IMediator mediator)
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

        // check tasks
        await validateTasks(tasks, taskStateIds, cancellationToken);

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

        return taskStates.Where(i => !i.Flag.HasFlag(EWorkflowTaskStateFlag.Inactive)).Select(i => i.Id).ToList();
    }

    public async Task validateTasks(IList<Contracts.WorkflowTask> tasks, IList<int> taskStateIds, CancellationToken cancellationToken)
    {
        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellationToken);
        var taskTypeIds = taskTypes.Select(i => i.Id).ToArray();

        var tasksWithInvalidTypeId = tasks.Where(t => !taskTypeIds.Contains(t.TaskTypeId));
        var tasksWithInvalidStateId = tasks.Where(t => !taskStateIds.Contains(t.StateIdSb));

        if (tasksWithInvalidTypeId.Any())
        {
            var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskId);
            throw new CisValidationException(ErrorCodeMapper.WfTaskValidationFailed1, ErrorCodeMapper.GetMessage(ErrorCodeMapper.WfTaskValidationFailed1, string.Join(",", taskIds)));
        }

        if (tasksWithInvalidStateId.Any())
        {
            var taskIds = tasksWithInvalidStateId.Select(t => t.TaskId);
            throw new CisValidationException(ErrorCodeMapper.WfTaskValidationFailed2, ErrorCodeMapper.GetMessage(ErrorCodeMapper.WfTaskValidationFailed2, string.Join(",", taskIds)));
        }
    }
}