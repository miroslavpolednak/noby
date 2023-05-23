using CIS.Core.Attributes;
using CIS.Core.Security;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.UserService.Clients;
using NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.Amendments;
using NOBY.Api.Endpoints.Cases.GetTaskList.Dto;
using static DomainServices.CodebookService.Contracts.v1.WorkflowTaskStatesNobyResponse.Types;
using _Case = DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Cases.Dto;

[SelfService, ScopedService]
public class WorkflowMapper
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    
    public async Task<WorkflowTask> Map(_Case.WorkflowTask task, CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var workflowState = GetWorkflowState(task);
        var taskState = taskStates.First(s => s.Id == (int)workflowState);

        return Map(task, taskState);
    }

    public async Task<List<WorkflowTask>> Map(List<_Case.WorkflowTask> tasks, CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var list = new List<Dto.WorkflowTask>();
        
        foreach (var task in tasks)
        {
            var workflowState = GetWorkflowState(task);
            var taskState = taskStates.First(s => s.Id == (int)workflowState);
            list.Add(Map(task, taskState));
        }

        return list;
    }

    public async Task<GetTaskDetail.Dto.WorkflowTaskDetail> Map(_Case.WorkflowTask task, TaskDetailItem taskDetailItem, CancellationToken cancellationToken)
    {
        var performer = await _codebookService.GetOperator(task.PerformerLogin, cancellationToken);

        var taskDetail = new GetTaskDetail.Dto.WorkflowTaskDetail
        {
            TaskIdSB = task.TaskIdSb,
            PerformerLogin = performer?.PerformerLogin,
            PerformerName = performer?.PerformerName,
            ProcessNameLong = taskDetailItem.ProcessNameLong ?? string.Empty,
            Amendments = Map(task, taskDetailItem)
        };

        taskDetail.TaskCommunication.AddRange(taskDetailItem.TaskCommunication.Select(Map));
        
        return taskDetail;
    }

    private static object Map(_Case.WorkflowTask task, TaskDetailItem taskDetailItem) =>
        taskDetailItem.AmendmentsCase switch
        {
            TaskDetailItem.AmendmentsOneofCase.Request => Map(taskDetailItem.Request),
            TaskDetailItem.AmendmentsOneofCase.Signing => Map(task, taskDetailItem.Signing),
            TaskDetailItem.AmendmentsOneofCase.ConsultationData => Map(taskDetailItem.ConsultationData),
            _ => throw new ArgumentOutOfRangeException()
        };
    

    private static AmendmentsRequest Map(AmendmentRequest request) => new()
    {
        OrderId = request.OrderId,
        SentToCustomer = request.SentToCustomer
    };

    private static AmendmentsSigning Map(_Case.WorkflowTask task, AmendmentSigning signing) => new()
    {
        SignatureType = GetSignatureType(task),
        Expiration = signing.Expiration,
        FormId = signing.FormId,
        DocumentForSigning = signing.DocumentForSigning,
        ProposalForEntry = signing.ProposalForEntry
    };
    
    private static AmendmentsConsultationData Map(AmendmentConsultationData consultationData) => new()
    {
        OrderId = consultationData.OrderId
    };

    private NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.TaskCommunicationItem Map(TaskCommunicationItem taskCommunicationItem) => new()
    {
        TaskRequest = taskCommunicationItem.TaskRequest,
        TaskResponse = taskCommunicationItem.TaskResponse
    };
    
    private static Dto.WorkflowTask Map(_Case.WorkflowTask task, WorkflowTaskStatesNobyItem taskState) => new()
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
    
    
    private static State GetWorkflowState(_Case.WorkflowTask task)
    {
        if (task.Cancelled)
            return State.Cancelled;

        if (task.StateIdSb == 30)
            return State.Completed;

        return task.TaskTypeId switch
        {
            1 => GetRequestState(task),
            2 => GetPriceExceptionState(task),
            3 or 7 => State.Sent,
            6 => GetSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static State GetRequestState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => State.ForProcessing,
            2 => State.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static State GetPriceExceptionState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => State.Sent,
            2 => State.Completed,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static SignatureType GetSignatureType(_Case.WorkflowTask task) =>
        task.SignatureType switch
        {
            "paper" => SignatureType.Paper,
            "digital" => SignatureType.Digital,
            _ => throw new ArgumentOutOfRangeException()
        };
    
    private static State GetSignatureState(_Case.WorkflowTask task) =>
        task.SignatureType switch
        {
            "paper" => GetPaperSignatureState(task),
            "digital" => GetDigitalSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static State GetDigitalSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => State.ForProcessing,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static State GetPaperSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => State.ForProcessing,
            2 => State.OperationalSupport,
            3 => State.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    public WorkflowMapper(
        ICodebookServiceClient codebookService,
        IUserServiceClient userService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _userService = userService;
        _currentUserAccessor = currentUserAccessor;
    }
}