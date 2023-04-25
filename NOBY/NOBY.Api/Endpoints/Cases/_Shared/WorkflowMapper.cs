using CIS.Core.Attributes;
using CIS.Core.Security;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStatesNoby;
using DomainServices.UserService.Clients;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;
using WorkflowTask = DomainServices.CaseService.Contracts.WorkflowTask;


namespace NOBY.Api.Endpoints.Cases.Dto;

[SelfService, ScopedService]
public class WorkflowMapper
{
    private readonly ICodebookServiceClients _codebookService;
    private readonly IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    
    public async Task<NOBY.Api.Endpoints.Cases.GetTaskList.Dto.WorkflowTaskNew> Map(
        WorkflowTask task,
        CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var workflowState = await GetWorkflowState(task, cancellationToken);
        var taskState = taskStates.First(s => s.Id == (int)workflowState);

        return MapInternal(task, taskState);
    }

    public async Task<List<NOBY.Api.Endpoints.Cases.GetTaskList.Dto.WorkflowTaskNew>> Map(
        List<WorkflowTask> tasks,
        CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var list = new List<NOBY.Api.Endpoints.Cases.GetTaskList.Dto.WorkflowTaskNew>();
        foreach (var task in tasks)
        {
            var workflowState = await GetWorkflowState(task, cancellationToken);
            var taskState = taskStates.First(s => s.Id == (int)workflowState);
            list.Add(MapInternal(task, taskState));
        }

        return list;
    }

    public async Task<NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.WorkflowTaskDetail> Map(WorkflowTask task, TaskDetailItem taskDetailItem, CancellationToken cancellationToken)
    {
        var performer = await _codebookService.GetOperator(task.PerformerLogin, cancellationToken);

        var taskDetail = new NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.WorkflowTaskDetail
        {
            TaskIdSB = task.TaskIdSb,
            PerformerLogin = performer?.PerformerLogin ?? string.Empty,
            PerformerName = performer?.PerformerName ?? string.Empty,
            ProcessNameLong = taskDetailItem.ProcessNameLong ?? string.Empty,
            SentToCustomer = taskDetailItem.SentToCustomer ?? false,
            OrderId = taskDetailItem.OrderId ?? 0
        };

        taskDetail.TaskCommunication.AddRange(taskDetailItem.TaskCommunication.Select(Map));
        
        return taskDetail;
    }

    private NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.TaskCommunicationItem Map(TaskCommunicationItem taskCommunicationItem)
    {
        return new NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.TaskCommunicationItem
        {
            TaskRequest = taskCommunicationItem.TaskRequest,
            TaskResponse = taskCommunicationItem.TaskResponse
        };
    }
    
    private NOBY.Api.Endpoints.Cases.GetTaskList.Dto.WorkflowTaskNew MapInternal(WorkflowTask task,
        WorkflowTaskStateNobyItem taskState)
    {
        return new NOBY.Api.Endpoints.Cases.GetTaskList.Dto.WorkflowTaskNew
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
            StateIndicator = Enum.Parse<StateIndicators>(taskState.Indicator, true)
        };
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
    
    public WorkflowMapper(
        ICodebookServiceClients codebookService,
        IUserServiceClient userService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _userService = userService;
        _currentUserAccessor = currentUserAccessor;
    }
}