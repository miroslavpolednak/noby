using DomainServices.CodebookService.Clients;
using DomainServices.CaseService.Contracts;
using Grpc.Core;
using CIS.Infrastructure.gRPC;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskList;

internal sealed class GetTaskListHandler
    : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellation)
    {
        var caseId = (int)request.CaseId;

        // check if case exists
        //await _repository.EnsureExistingCase(caseId, cancellation);

        // load user
        var user = await _userService.GetUser(_userAccessor.User!.Id, cancellation);

        // load codebooks
        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellation);
        var taskStates = await _codebookService.WorkflowTaskStates(cancellation);

        var taskTypeIds = taskTypes.Select(i => i.Id).ToArray();
        var taskStateIds = taskStates.Where(i => !i.Flag.HasFlag(CodebookService.Contracts.Endpoints.WorkflowTaskStates.EWorkflowTaskStateFlag.Inactive)).Select(i => i.Id).ToArray();

        // request data
        var header = new EasSimulationHT.EasSimulationHTWrapper.WFS_Header { system = "NOBY", login = GetLogin(user) };
        var messsage = new EasSimulationHT.EasSimulationHTWrapper.WFS_Find_ByCaseId
        {
            case_id = caseId,
            task_state = taskStateIds,
            search_pattern = "LoanProcessSubtasks",
        };

        // load tasks
        var easTasks = await _easSimulationHTClient.FindTasks(header, messsage);
        var tasks = easTasks.Select(i => i.ToWorkflowTask()).ToArray();

        // check tasks
        CheckTasks(tasks, taskTypeIds, taskStateIds);

        // update active tasks
        var updateRequest = new UpdateActiveTasksRequest
        {
            CaseId = caseId
        };
        updateRequest.Tasks.AddRange(tasks.Select(i => i.ToUpdateTaskItem()));
        await _mediator.Send(updateRequest, cancellation);

        // response
        var response = new GetTaskListResponse();
        response.Tasks.AddRange(tasks);
        return response;
    }

    private static void CheckTasks(WorkflowTask[] tasks, int[] taskTypeIds, int[] taskStateIds)
    {
        var tasksWithInvalidTypeId = tasks.Where(t => !taskTypeIds.Contains(t.TypeId));
        var tasksWithInvalidStateId = tasks.Where(t => !taskStateIds.Contains(t.StateId));

        if (!(tasksWithInvalidTypeId.Any() || tasksWithInvalidStateId.Any()))
            return;

        var invalidTypeIds = tasksWithInvalidTypeId.Select(t => t.TypeId).Distinct();
        var invalidStateIds = tasksWithInvalidStateId.Select(t => t.StateId).Distinct();

        var message = string.Empty;

        if (tasksWithInvalidTypeId.Any())
        {
            var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskId);
            message += $"Found tasks [{string.Join(",", taskIds)}] with invalid TypeId [{string.Join(",", invalidTypeIds)}].";
        }

        if (tasksWithInvalidStateId.Any())
        {
            var taskIds = tasksWithInvalidStateId.Select(t => t.TaskId);
            message += $"Found tasks [{string.Join(",", taskIds)}] with invalid StateId [{string.Join(",", invalidStateIds)}].";
        }

        throw new CisValidationException(13008, message);
    }

    private static string GetLogin(UserService.Contracts.User user)
    {
        if (!string.IsNullOrWhiteSpace(user.CPM) && !string.IsNullOrWhiteSpace(user.ICP))
        {
            // "login": "CPM: 99811022 ICP: 128911022"
            return $"CPM: {user.CPM} ICP: {user.ICP}";
        }

        var identity = user.UserIdentifiers.FirstOrDefault()?.Identity;

        return identity ?? string.Empty;
    }

    private readonly ILogger<GetTaskListHandler> _logger;
    private readonly ICodebookServiceClients _codebookService;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly IMediator _mediator;

    public GetTaskListHandler(
        ILogger<GetTaskListHandler> logger,
        ICodebookServiceClients codebookService,
        UserService.Clients.IUserServiceClient userService,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        IMediator mediator
        )
    {
        _logger = logger;
        _codebookService = codebookService;
        _userService = userService;
        _easSimulationHTClient = easSimulationHTClient;
        _userAccessor = userAccessor;
        _mediator = mediator;
    }
}
