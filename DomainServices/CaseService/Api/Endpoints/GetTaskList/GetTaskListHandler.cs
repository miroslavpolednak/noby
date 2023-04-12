using DomainServices.CaseService.Api.Services;
using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskList;

internal sealed class GetTaskListHandler
    : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        // check if case exists
        await CheckIfCaseExists(request.CaseId, cancellationToken);

        // load codebooks
        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellationToken);

        var taskTypeIds = taskTypes.Select(i => i.Id).ToArray();
        var taskStateIds = await _commonDataProvider.GetValidTaskStateIds(cancellationToken);

        var sbRequest = new FindByCaseIdRequest
        {
            HeaderLogin = await _commonDataProvider.GetCurrentLogin(cancellationToken),
            CaseId = request.CaseId,
            TaskStates = taskStateIds,
            SearchPattern = "LoanProcessSubtasks"
        };

        var foundTasks = await _sbWebApiClient.FindTasksByCaseId(sbRequest, cancellationToken);

        var tasks = foundTasks.Tasks.Select(taskData => taskData.ToWorkflowTask()).ToList();

        // check tasks
        CheckTasks(tasks, taskTypeIds, taskStateIds);

        // update active tasks
        var updateRequest = new UpdateActiveTasksRequest
        {
            CaseId = request.CaseId
        };
        updateRequest.Tasks.AddRange(tasks.Select(i => i.ToUpdateTaskItem()));
        await _mediator.Send(updateRequest, cancellationToken);

        // response
        var response = new GetTaskListResponse();
        response.Tasks.AddRange(tasks);
        return response;
    }

    private static void CheckTasks(ICollection<WorkflowTask> tasks, int[] taskTypeIds, ICollection<int> taskStateIds)
    {
        var tasksWithInvalidTypeId = tasks.Where(t => !taskTypeIds.Contains(t.TaskTypeId));
        var tasksWithInvalidStateId = tasks.Where(t => !taskStateIds.Contains(t.StateIdSb));

        if (tasksWithInvalidTypeId.Any())
        {
            var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskId);
            throw new CisValidationException(ErrorCodeMapper.WfTaskValidationFailed1, string.Join(",", taskIds));
        }

        if (tasksWithInvalidStateId.Any())
        {
            var taskIds = tasksWithInvalidStateId.Select(t => t.TaskId);
            throw new CisValidationException(ErrorCodeMapper.WfTaskValidationFailed2, string.Join(",", taskIds));
        }
    }

    private async Task CheckIfCaseExists(long caseId, CancellationToken cancellationToken)
    {
        var caseExists = await _dbContext.Cases.AnyAsync(c => c.CaseId == caseId, cancellationToken);

        if (caseExists)
            return;

        throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, caseId);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly SbWebApiCommonDataProvider _commonDataProvider;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ISbWebApiClient _sbWebApiClient;
    private readonly IMediator _mediator;

    public GetTaskListHandler(
        CaseServiceDbContext dbContext,
        SbWebApiCommonDataProvider commonDataProvider,
        ICodebookServiceClients codebookService,
        ISbWebApiClient sbWebApiClient,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _commonDataProvider = commonDataProvider;
        _codebookService = codebookService;
        _sbWebApiClient = sbWebApiClient;
        _mediator = mediator;
    }
}
