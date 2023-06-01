using CIS.Core.Attributes;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Contracts.v1;
using DomainServices.CodebookService.Clients;
using NOBY.Api.Endpoints.Cases.GetTaskDetail.Dto.Amendments;
using static DomainServices.CodebookService.Contracts.v1.WorkflowTaskStatesNobyResponse.Types;
using _Case = DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Cases.Dto;

[SelfService, ScopedService]
public class WorkflowMapper
{
    private readonly ICodebookServiceClient _codebookService;

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

    public async Task<Dto.WorkflowTaskDetail> Map(_Case.WorkflowTask task, TaskDetailItem taskDetailItem, CancellationToken cancellationToken)
    {
        var performer = await _codebookService.GetOperator(task.PerformerLogin, cancellationToken);

        var taskDetail = new Dto.WorkflowTaskDetail
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

    private TaskCommunicationItem Map(DomainServices.CaseService.Contracts.TaskCommunicationItem taskCommunicationItem) => new()
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
    
    
    private static GetTaskList.Dto.State GetWorkflowState(_Case.WorkflowTask task)
    {
        if (task.Cancelled)
            return GetTaskList.Dto.State.Cancelled;

        if (task.StateIdSb == 30)
            return GetTaskList.Dto.State.Completed;

        return task.TaskTypeId switch
        {
            1 => GetRequestState(task),
            2 => GetPriceExceptionState(task),
            3 or 7 => GetTaskList.Dto.State.Sent,
            6 => GetSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static GetTaskList.Dto.State GetRequestState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => GetTaskList.Dto.State.ForProcessing,
            2 => GetTaskList.Dto.State.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static GetTaskList.Dto.State GetPriceExceptionState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => GetTaskList.Dto.State.Sent,
            2 => GetTaskList.Dto.State.Completed,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static SignatureType GetSignatureType(_Case.WorkflowTask task) =>
        task.SignatureType switch
        {
            "paper" => SignatureType.Paper,
            "digital" => SignatureType.Digital,
            _ => throw new ArgumentOutOfRangeException()
        };
    
    private static GetTaskList.Dto.State GetSignatureState(_Case.WorkflowTask task) =>
        task.SignatureType switch
        {
            "paper" => GetPaperSignatureState(task),
            "digital" => GetDigitalSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static GetTaskList.Dto.State GetDigitalSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => GetTaskList.Dto.State.ForProcessing,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static GetTaskList.Dto.State GetPaperSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => GetTaskList.Dto.State.ForProcessing,
            2 => GetTaskList.Dto.State.OperationalSupport,
            3 => GetTaskList.Dto.State.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    public WorkflowMapper(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}