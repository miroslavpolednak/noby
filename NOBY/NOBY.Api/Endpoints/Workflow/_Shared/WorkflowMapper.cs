using CIS.Core.Attributes;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using NOBY.Api.Endpoints.Workflow.GetTaskDetail.Dto.Amendments;
using NOBY.Api.Endpoints.Workflow.GetTaskList.Dto;
using static DomainServices.CodebookService.Contracts.v1.WorkflowTaskStatesNobyResponse.Types;
using _Case = DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Workflow.Dto;

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
        var list = new List<WorkflowTask>();

        foreach (var task in tasks)
        {
            var workflowState = GetWorkflowState(task);
            var taskState = taskStates.First(s => s.Id == (int)workflowState);
            list.Add(Map(task, taskState));
        }

        return list;
    }

    public async Task<WorkflowTaskDetail> Map(_Case.WorkflowTask task, TaskDetailItem taskDetailItem, CancellationToken cancellationToken)
    {
        var performer = await _codebookService.GetOperator(task.PerformerLogin, cancellationToken);
        var decisionTypes = await _codebookService.WorkflowPriceExceptionDecisionTypes(cancellationToken);
        var loanInterestRateAnnouncedTypes = await _codebookService.LoanInterestRateAnnouncedTypes(cancellationToken);
        
        var taskDetail = new WorkflowTaskDetail
        {
            TaskIdSB = task.TaskIdSb,
            PerformerLogin = performer?.PerformerLogin,
            PerformerName = performer?.PerformerName,
            PerformerCode = performer?.PerformerCode,
            ProcessNameLong = taskDetailItem.ProcessNameLong ?? string.Empty,
            Amendments = Map(task, taskDetailItem, decisionTypes, loanInterestRateAnnouncedTypes)
        };

        taskDetail.TaskCommunication.AddRange(taskDetailItem.TaskCommunication.Select(Map));

        return taskDetail;
    }

    private static object Map(_Case.WorkflowTask task, TaskDetailItem taskDetailItem,
        List<GenericCodebookResponse.Types.GenericCodebookItem> decisionTypes,
        List<GenericCodebookResponse.Types.GenericCodebookItem> loanInterestRateAnnouncedTypes) =>
        taskDetailItem.AmendmentsCase switch
        {
            TaskDetailItem.AmendmentsOneofCase.Request => Map(taskDetailItem.Request),
            TaskDetailItem.AmendmentsOneofCase.Signing => Map(task, taskDetailItem.Signing),
            TaskDetailItem.AmendmentsOneofCase.ConsultationData => Map(taskDetailItem.ConsultationData),
            TaskDetailItem.AmendmentsOneofCase.PriceException => Map(taskDetailItem.PriceException, decisionTypes, loanInterestRateAnnouncedTypes),
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

    private static AmendmentsPriceException Map(
        AmendmentPriceException amendmentPriceException,
        List<GenericCodebookResponse.Types.GenericCodebookItem> decisionTypes,
        List<GenericCodebookResponse.Types.GenericCodebookItem> loanInterestRateAnnouncedTypes) => new()
    {
        Expiration = amendmentPriceException.Expiration,
        Decision = GetDecision(amendmentPriceException, decisionTypes),
        Fees = amendmentPriceException.Fees.Select(Map).ToList(),
        LoanInterestRate = Map(amendmentPriceException.LoanInterestRate, loanInterestRateAnnouncedTypes)
    };
    
    private static Fee Map(AmendmentPriceException.Types.FeesItem feeItem) => new()
    {
        DiscountPercentage = feeItem.DiscountPercentage,
        FeeId = feeItem.FeeId,
        FinalSum = feeItem.FinalSum,
        TariffSum = feeItem.TariffSum
    };

    private static LoanInterestRates Map(
        AmendmentPriceException.Types.LoanInterestRateItem loanInterestRate,
        List<GenericCodebookResponse.Types.GenericCodebookItem> loanInterestRateAnnouncedTypes) => new()
    {
        LoanInterestRate = loanInterestRate.LoanInterestRate,
        LoanInterestRateDiscount = loanInterestRate.LoanInterestRateDiscount,
        LoanInterestRateProvided = loanInterestRate.LoanInterestRateProvided,
        LoanInterestRateAnnouncedTypeName = loanInterestRateAnnouncedTypes
            .FirstOrDefault(t => t.Id == loanInterestRate.LoanInterestRateAnnouncedType)?
            .Name ?? string.Empty
    };
    
    private static TaskCommunicationItem Map(_Case.TaskCommunicationItem taskCommunicationItem) => new()
    {
        TaskRequest = taskCommunicationItem.TaskRequest,
        TaskResponse = taskCommunicationItem.TaskResponse
    };

    private static WorkflowTask Map(_Case.WorkflowTask task, WorkflowTaskStatesNobyItem taskState) => new()
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

    private static string GetDecision(
        AmendmentPriceException amendmentPriceException,
        List<GenericCodebookResponse.Types.GenericCodebookItem> decisionTypes)
    {
        return decisionTypes
            .FirstOrDefault(t => t.Id == amendmentPriceException.DecisionId)?.Name ?? string.Empty;
    }
    
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

    public WorkflowMapper(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}