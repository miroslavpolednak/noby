using DomainServices.CodebookService.Abstraction;
using DomainServices.CaseService.Contracts;
using Grpc.Core;
using CIS.Infrastructure.gRPC;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetTaskListHandler
    : IRequestHandler<Dto.GetTaskListMediatrRequest, GetTaskListResponse>
{

    #region Construction

    private readonly ILogger<GetTaskListHandler> _logger;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly UserService.Abstraction.IUserServiceAbstraction _userService;
    private readonly EasSimulationHT.IEasSimulationHTClient _easSimulationHTClient;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly IMediator _mediator;

    public GetTaskListHandler(
        ILogger<GetTaskListHandler> logger,
        Repositories.CaseServiceRepository repository,
        ICodebookServiceAbstraction codebookService,
        UserService.Abstraction.IUserServiceAbstraction userService,
        EasSimulationHT.IEasSimulationHTClient easSimulationHTClient,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        IMediator mediator
        )
    {
        _logger = logger;
        _repository = repository;
        _codebookService = codebookService;
        _userService = userService;
        _easSimulationHTClient = easSimulationHTClient;
        _userAccessor = userAccessor;
        _mediator = mediator;
    }

    #endregion

    public async Task<GetTaskListResponse> Handle(Dto.GetTaskListMediatrRequest request, CancellationToken cancellation)
    {
        var caseId = (int)request.CaseId;

        // check if case exists
        await _repository.EnsureExistingCase(caseId, cancellation);

        // TODO: _userAccessor.User = NULL !!!
        var userId = 1; //_userAccessor.User!.Id;
        var user = ServiceCallResult.ResolveAndThrowIfError<UserService.Contracts.User>(await _userService.GetUser(userId, cancellation));

        // load codebooks
        var taskTypes = await _codebookService.WorkflowTaskTypes(cancellation);
        var taskStates = await _codebookService.WorkflowTaskStates(cancellation);

        var taskTypeIds = taskTypes.Select(i => i.Id).ToArray();
        var taskStateIds = taskStates.Select(i => i.Id).Where(i => i < 30).ToArray(); // ??? //TODO: impelement codebook extension by flag

        // request data
        var header = new EasSimulationHT.EasSimulationHTWrapper.WFS_Header { system = "NOBY", login = GetLogin(user) };
        var messsage = new EasSimulationHT.EasSimulationHTWrapper.WFS_Find_ByCaseId
        {
            case_id = caseId,
            task_state = taskStateIds, // new int[] { 0, 10, 20, 22, 24, 26 },
        };

        // load tasks
        var easTasks = ResolveFindTasks(await _easSimulationHTClient.FindTasks(header, messsage));
        var tasks = easTasks.Select(i => i.ToWorkflowTask()).ToArray();

        // check tasks
        CheckTasks(tasks, taskTypeIds, taskStateIds);

        // update active tasks
        var items = tasks.Select(i => i.ToUpdateTaskItem()).ToArray();
        await _mediator.Send(new Dto.UpdateActiveTasksMediatrRequest(caseId, items), cancellation);

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

        var message = String.Empty;

        if (tasksWithInvalidTypeId.Any())
        {
            var taskIds = tasksWithInvalidTypeId.Select(t => t.TaskId);
            message += $"Found tasks [{String.Join(",", taskIds)}] with invalid TypeId [{String.Join(",", invalidTypeIds)}].";
        }

        if (tasksWithInvalidStateId.Any())
        {
            var taskIds = tasksWithInvalidStateId.Select(t => t.TaskId);
            message += $"Found tasks [{String.Join(",", taskIds)}] with invalid StateId [{String.Join(",", invalidStateIds)}].";
        }

        throw new CisValidationException(99999, message); //TODO: ErrorCode
    }

    private static string GetLogin(UserService.Contracts.User user)
    {
        if (!String.IsNullOrWhiteSpace(user.CPM) && !String.IsNullOrWhiteSpace(user.ICP))
        {
            return $"{user.CPM}/{user.ICP}";
        }

        var identity = user.UserIdentifiers.FirstOrDefault()?.Identity;

        return identity ?? String.Empty;
    }

    private static ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.WFS_FindItem[] ResolveFindTasks (IServiceCallResult result) =>
     result switch
     {
         SuccessfulServiceCallResult<ExternalServices.EasSimulationHT.V6.EasSimulationHTWrapper.WFS_FindItem[]> r => r.Model,
         ErrorServiceCallResult err => throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, err.Errors[0].Message, err.Errors[0].Key),
         _ => throw new NotImplementedException("FindTasks")
     };

}
