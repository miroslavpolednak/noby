using CIS.Core.Security;
using CIS.Infrastructure.gRPC.CisTypes;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;
using WorkflowTask = DomainServices.CaseService.Contracts.WorkflowTask;

namespace NOBY.Api.Endpoints.Cases.GetTaskList;

internal sealed class GetTaskListHandler : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetTaskListHandler(
        DomainServices.CodebookService.Clients.ICodebookServiceClients codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        DomainServices.UserService.Clients.IUserServiceClient userService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _userService = userService;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        var workflowTasks = LoadWorkflowTasks(request.CaseId, cancellationToken);
        var workflowProcesses = LoadWorkflowProcesses(request.CaseId, cancellationToken);

        return new GetTaskListResponse
        {
            Tasks = await workflowTasks,
            Processes = await workflowProcesses
        };
    }

    private async Task<List<Dto.WorkflowTask>?> LoadWorkflowTasks(long caseId, CancellationToken cancellationToken)
    {
        var tasks = await _caseService.GetTaskList(caseId, cancellationToken);
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);

        if (!tasks.Any())
            return default;

        var list = new List<Dto.WorkflowTask>();

        foreach (var task in tasks)
        {
            var workflowState = await GetWorkflowState(task, cancellationToken);

            var taskState = taskStates.First(s => s.Id == (int)workflowState);

            var newTask = new Dto.WorkflowTask
            {
                TaskId = task.TaskId,
                CreatedOn = task.CreatedOn,
                TaskTypeId = task.TaskTypeId,
                TaskTypeName = task.TaskTypeName,
                TaskSubtypeName = task.TaskSubtypeName,
                ProcessId = task.ProcessId,
                ProcessNameShort = task.ProcessNameShort,
                StateId = taskState.Id,
                StateName = taskState.Name,
                StateFilter = Enum.Parse<StateFilter>(taskState.Filter, true),
                StateIndicator = Enum.Parse<StateIndicator>(taskState.Indicator, true)
            };

            list.Add(newTask);
        }

        return list;
    }

    private async Task<List<WorkflowProcess>> LoadWorkflowProcesses(long caseId, CancellationToken cancellationToken)
    {
        var processes = await _caseService.GetProcessList(caseId, cancellationToken);
        var processStates = await _codebookService.WorkflowProcessStatesNoby(cancellationToken);

        return processes.Select(p => new WorkflowProcess
        {
            ProcessId = p.ProcessId,
            CreatedOn = p.CreatedOn,
            ProcessNameLong = p.ProcessNameLong,
            StateName = p.StateName,
            StateIndicator = GetStateIndicator(p.StateName)
        }).ToList();

        StateIndicator GetStateIndicator(string stateName) => Enum.Parse<StateIndicator>(processStates.First(s => s.Name.Equals(stateName, StringComparison.InvariantCultureIgnoreCase)).Indicator);
    }

    private async Task<State> GetWorkflowState(WorkflowTask task, CancellationToken cancellationToken)
    {
        if (task.Cancelled)
            return State.Cancelled;

        if (task.StateIdSb == 30)
            return State.Completed;

        return task.TaskTypeId switch
        {
            1 => GetRequestState(task.PhaseTypeId),
            2 => GetPriceExceptionState(task.PhaseTypeId),
            3 or 7 => State.Sent,
            6 => await GetSignatureState(task, cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static State GetRequestState(int phaseTypeId) =>
        phaseTypeId switch
        {
            1 => State.ForProcessing,
            2 => State.Sent,
            _ => throw new ArgumentOutOfRangeException(nameof(phaseTypeId), phaseTypeId, null)
        };

    private static State GetPriceExceptionState(int phaseTypeId) =>
        phaseTypeId switch
        {
            1 => State.Sent,
            2 => State.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(phaseTypeId), phaseTypeId, null)
        };

    private async Task<State> GetSignatureState(WorkflowTask task, CancellationToken cancellationToken) =>
        task.SignatureType switch
        {
            "digital" => GetDigitalSignatureState(task.PhaseTypeId),
            "paper" => await GetPaperSignatureState(task, cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static State GetDigitalSignatureState(int phaseTypeId) =>
        phaseTypeId switch
        {
            1 => State.ForProcessing,
            2 => State.Sent,
            _ => throw new ArgumentOutOfRangeException(nameof(phaseTypeId), phaseTypeId, null)
        };

    private async Task<State> GetPaperSignatureState(WorkflowTask task, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);

        if (user.UserIdentifiers.Any(i => i.IdentityScheme == UserIdentity.Types.UserIdentitySchemes.BrokerId))
        {
            return task.PhaseTypeId switch
            {
                1 => State.ForProcessing,
                2 => State.OperationalSupport,
                3 => State.Sent,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return task.PhaseTypeId switch
        {
            1 => State.ForProcessing,
            2 => State.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
