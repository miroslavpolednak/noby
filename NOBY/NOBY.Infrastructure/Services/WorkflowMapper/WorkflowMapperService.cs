using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using _Case = DomainServices.CaseService.Contracts;
using NOBY.Dto.Workflow;

namespace NOBY.Infrastructure.Services.WorkflowMapper;

[SelfService, TransientService]
public sealed class WorkflowMapperService
{
    public async Task<Dto.Workflow.WorkflowTask> Map(_Case.WorkflowTask task, CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var workflowState = GetWorkflowState(task);
        var taskState = taskStates.First(s => s.Id == (int)workflowState);

        return Map(task, taskState);
    }

    public async Task<List<Dto.Workflow.WorkflowTask>> Map(List<_Case.WorkflowTask> tasks, CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);
        var list = new List<Dto.Workflow.WorkflowTask>();

        foreach (var task in tasks)
        {
            var workflowState = GetWorkflowState(task);
            var taskState = taskStates.First(s => s.Id == (int)workflowState);
            list.Add(Map(task, taskState));
        }

        return list;
    }

    public async Task<WorkflowTaskDetail> Map(_Case.WorkflowTask task, _Case.TaskDetailItem taskDetailItem, CancellationToken cancellationToken)
    {
        GetOperatorResponse? performer = null;
        try
        {
            performer = await _codebookService.GetOperator(task.PerformerLogin, cancellationToken);
        }
        catch { } // operator neni povinny

        var taskDetail = new WorkflowTaskDetail
        {
            TaskIdSB = task.TaskIdSb,
            PerformerLogin = performer?.PerformerLogin,
            PerformerName = performer?.PerformerName,
            PerformerCode = performer?.PerformerCode,
            ProcessNameLong = taskDetailItem.ProcessNameLong ?? string.Empty,
            Amendments = Map(task, taskDetailItem)
        };

        taskDetail.TaskCommunication.AddRange(taskDetailItem.TaskCommunication.Select(Map));

        return taskDetail;
    }

    private static object Map(_Case.WorkflowTask task, _Case.TaskDetailItem taskDetailItem) =>
        taskDetailItem.AmendmentsCase switch
        {
            _Case.TaskDetailItem.AmendmentsOneofCase.Request => Map(taskDetailItem.Request),
            _Case.TaskDetailItem.AmendmentsOneofCase.Signing => Map(task, taskDetailItem.Signing),
            _Case.TaskDetailItem.AmendmentsOneofCase.ConsultationData => Map(taskDetailItem.ConsultationData),
            _ => throw new ArgumentOutOfRangeException()
        };


    private static AmendmentsRequest Map(_Case.AmendmentRequest request) => new()
    {
        OrderId = request.OrderId,
        SentToCustomer = request.SentToCustomer
    };

    private static AmendmentsSigning Map(_Case.WorkflowTask task, _Case.AmendmentSigning signing) => new()
    {
        SignatureType = GetSignatureType(task),
        Expiration = signing.Expiration,
        FormId = signing.FormId,
        DocumentForSigning = signing.DocumentForSigning,
        ProposalForEntry = signing.ProposalForEntry
    };

    private static AmendmentsConsultationData Map(_Case.AmendmentConsultationData consultationData) => new()
    {
        OrderId = consultationData.OrderId
    };

    private TaskCommunicationItem Map(_Case.TaskCommunicationItem taskCommunicationItem) => new()
    {
        TaskRequest = taskCommunicationItem.TaskRequest,
        TaskResponse = taskCommunicationItem.TaskResponse
    };

    private static Dto.Workflow.WorkflowTask Map(_Case.WorkflowTask task, WorkflowTaskStatesNobyResponse.Types.WorkflowTaskStatesNobyItem taskState) => new()
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
        StateFilter = Enum.Parse<StateFilters>(taskState.Filter, true),
        StateIndicator = Enum.Parse<StateIndicators>(taskState.Indicator, true)
    };


    private static WorkflowTaskStates GetWorkflowState(_Case.WorkflowTask task)
    {
        if (task.Cancelled)
            return WorkflowTaskStates.Cancelled;

        if (task.StateIdSb == 30)
            return WorkflowTaskStates.Completed;

        return task.TaskTypeId switch
        {
            1 => GetRequestState(task),
            2 => GetPriceExceptionState(task),
            3 or 7 => WorkflowTaskStates.Sent,
            6 => GetSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static WorkflowTaskStates GetRequestState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.ForProcessing,
            2 => WorkflowTaskStates.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static WorkflowTaskStates GetPriceExceptionState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.Sent,
            2 => WorkflowTaskStates.Completed,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static SignatureType GetSignatureType(_Case.WorkflowTask task) =>
        task.SignatureType switch
        {
            "paper" => SignatureType.Paper,
            "digital" => SignatureType.Digital,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static WorkflowTaskStates GetSignatureState(_Case.WorkflowTask task) =>
        task.SignatureType switch
        {
            "paper" => GetPaperSignatureState(task),
            "digital" => GetDigitalSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static WorkflowTaskStates GetDigitalSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.ForProcessing,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static WorkflowTaskStates GetPaperSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.ForProcessing,
            2 => WorkflowTaskStates.OperationalSupport,
            3 => WorkflowTaskStates.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    private readonly ICodebookServiceClient _codebookService;

    public WorkflowMapperService(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}