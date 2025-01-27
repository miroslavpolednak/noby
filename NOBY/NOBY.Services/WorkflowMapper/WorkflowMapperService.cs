﻿using DomainServices.CodebookService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using CIS.Core.Security;
using NOBY.Infrastructure.Security;
using NOBY.Infrastructure.ErrorHandling;
using _Perm = DomainServices.UserService.Clients.Authorization.UserPermissions;
using NOBY.ApiContracts;

namespace NOBY.Services.WorkflowMapper;

[TransientService, AsImplementedInterfacesService]
internal sealed class WorkflowMapperService(
    ICodebookServiceClient _codebookService, 
    ICurrentUserAccessor _userAccessor)
        : IWorkflowMapperService
{
    public SharedTypesWorkflowProcess MapProcess(_Case.ProcessTask task)
    {
        return new SharedTypesWorkflowProcess
        {
            ProcessId = task.ProcessId,
            CreatedOn = task.CreatedOn,
            ProcessNameLong = task.ProcessNameLong,
            StateName = task.StateName,
            ProcessTypeId = task.ProcessTypeId,
            StateIndicator = task.StateIndicator.HasValue ? (EnumStateIndicators)task.StateIndicator : EnumStateIndicators.Unknown //TODO co je default stav?
        };
    }

    public async Task<SharedTypesWorkflowTask> MapTask(_Case.WorkflowTask task, CancellationToken cancellationToken = default)
    {
        var taskState = (await _codebookService.WorkflowTaskStatesNoby(cancellationToken))
            .First(s => s.Id == (int)getWorkflowState(task));

        return new()
        {
            TaskId = task.TaskId,
            Cancelled = task.Cancelled,
            CreatedOn = task.CreatedOn,
            TaskTypeId = task.TaskTypeId,
            TaskTypeName = task.TaskTypeName,
            TaskSubtypeName = task.TaskSubtypeName,
            ProcessId = task.ProcessId,
            ProcessNameShort = task.ProcessNameShort,
            StateId = taskState.Id,
            StateName = taskState.Name,
            StateFilter = (EnumWorkflowTaskStateFilters)taskState.Filter,
            StateIndicator = (EnumStateIndicators)taskState.Indicator
        };
    }

    public async Task<SharedTypesWorkflowTaskDetail> MapTaskDetail(_Case.WorkflowTask task, _Case.TaskDetailItem taskDetailItem, CancellationToken cancellationToken = default)
    {
        return new SharedTypesWorkflowTaskDetail
        {
            TaskIdSB = task.TaskIdSb,
            PerformerCode = taskDetailItem.PerformerCode,
            PerformerName = taskDetailItem.PerformanName,
            ProcessNameLong = taskDetailItem.ProcessNameLong ?? string.Empty,
            TaskCommunication = taskDetailItem.TaskCommunication?.Select(t => new SharedTypesWorkflowTaskCommunicationItem
            {
                TaskRequest = t.TaskRequest,
                TaskResponse = t.TaskResponse
            }).ToList(),
            Amendments = taskDetailItem.AmendmentsCase switch
            {
                _Case.TaskDetailItem.AmendmentsOneofCase.Request => SharedTypesWorkflowTaskDetailAmendmentsOneOf.Create(mapAmendmentsRequest(taskDetailItem.Request)),
                _Case.TaskDetailItem.AmendmentsOneofCase.Signing => SharedTypesWorkflowTaskDetailAmendmentsOneOf.Create(await mapAmendmentsSigning(task, taskDetailItem.Signing, cancellationToken)),
                _Case.TaskDetailItem.AmendmentsOneofCase.ConsultationData => SharedTypesWorkflowTaskDetailAmendmentsOneOf.Create(mapAmendmentsConsultationData(taskDetailItem.ConsultationData)),
                _Case.TaskDetailItem.AmendmentsOneofCase.PriceException => SharedTypesWorkflowTaskDetailAmendmentsOneOf.Create(await mapAmendmentsPriceException(task.DecisionId, taskDetailItem.PriceException, cancellationToken)),
                _ => null
            }
        };
    }

    private static SharedTypesWorkflowAmendmentsRequest mapAmendmentsRequest(_Case.AmendmentRequest request)
        => new()
        {
            OrderId = request.OrderId,
            SentToCustomer = request.SentToCustomer
        };

    private async Task<SharedTypesWorkflowAmendmentsSigning> mapAmendmentsSigning(_Case.WorkflowTask task, _Case.AmendmentSigning signing, CancellationToken cancellationToken)
    {
        var eaCodeMain = (await _codebookService.EaCodesMain(cancellationToken)).FirstOrDefault(t => t.Id == signing.EACodeMain);
        // stav tasku
        var stateId = getWorkflowState(task);
        
        // helper props
        bool hasPaperSig = _userAccessor.HasPermission(_Perm.WFL_TASK_DETAIL_PaperSigningDocuments);
        bool hasDigitalSig = _userAccessor.HasPermission(_Perm.WFL_TASK_DETAIL_DigitalSigningDocuments);
        bool hasAttachSig = _userAccessor.HasPermission(_Perm.WFL_TASK_DETAIL_SigningAttachments);
        // ma alespon nejake podepisovaci pravo
        bool hasSigningPermissions = hasPaperSig || hasDigitalSig;
        
        bool removeProposalForEntry = _amendmentsSigningStates.Contains(stateId) || !hasSigningPermissions;
        bool sigLinkVisible = !_amendmentsSigningSignatureLinkStates.Contains(stateId) 
            && ((hasPaperSig && task.SignatureTypeId == (int)SignatureTypes.Paper) || (hasDigitalSig && task.SignatureTypeId == (int)SignatureTypes.Electronic));
        bool removeDocForSigning = stateId == WorkflowTaskStates.OperationalSupport && hasSigningPermissions && !sigLinkVisible;
        bool sendButtonVisible = stateId switch
        {
            WorkflowTaskStates.OperationalSupport => hasAttachSig,
            WorkflowTaskStates.ForProcessing => !hasPaperSig && (hasAttachSig || (hasDigitalSig && task.SignatureTypeId == (int)SignatureTypes.Paper)),
            _ => false
        }; ;

        return new()
        {
            SignatureTypeId = task.SignatureTypeId,
            Expiration = signing.Expiration,
            FormId = signing.FormId,
            DocumentForSigning = removeDocForSigning ? "" : signing.DocumentForSigning,
            DocumentForSigningType = signing.DocumentForSigningType,
            ProposalForEntry = removeProposalForEntry ? "" : (signing.ProposalForEntry == null || signing.ProposalForEntry.Count == 0 ? "" : signing.ProposalForEntry[0]),
            SignatureLinkVisible = sigLinkVisible,
            SendButtonVisible = sendButtonVisible,
            EaCodeMain = eaCodeMain == null ? null : new()
            {
                Id = eaCodeMain.Id,
                DocumentType = eaCodeMain.Name,
                Category = eaCodeMain.Category
            }
        };
    }
    
    private static SharedTypesWorkflowAmendmentsConsultationData mapAmendmentsConsultationData(_Case.AmendmentConsultationData consultationData) 
        => new()
        {
            OrderId = consultationData.OrderId,
            TaskSubtypeId = consultationData.TaskSubtypeId
        };

    private async Task<SharedTypesWorkflowAmendmentsPriceException> mapAmendmentsPriceException(int? taskDecisionId, _Case.AmendmentPriceException amendmentPriceException, CancellationToken cancellationToken)
    {
        var decisionTypes = await _codebookService.WorkflowPriceExceptionDecisionTypes(cancellationToken);
        var loanInterestRateAnnouncedTypes = await _codebookService.LoanInterestRateAnnouncedTypes(cancellationToken);
        var fees = await _codebookService.Fees(cancellationToken);

        return new()
        {
            Expiration = amendmentPriceException.Expiration,
            Decision = decisionTypes
                .FirstOrDefault(t => t.Id == taskDecisionId)?
                .Name ?? string.Empty,
            Fees = amendmentPriceException.Fees
            .Select(t => new SharedTypesWorkflowAmendmentsPriceExceptionFee()
            {
                DiscountPercentage = t.DiscountPercentage,
                FeeName = fees.FirstOrDefault(f => f.Id == Convert.ToInt32(t.FeeId))?.ShortName ?? string.Empty,
                FinalSum = t.FinalSum,
                TariffSum = t.TariffSum
            })
            .ToList(),
            LoanInterestRate = new()
            {
                LoanInterestRate = (decimal?)amendmentPriceException.LoanInterestRate.LoanInterestRate ?? 0,
                LoanInterestRateDiscount = (decimal?)amendmentPriceException.LoanInterestRate.LoanInterestRateDiscount ?? 0,
                LoanInterestRateProvided = (decimal?)amendmentPriceException.LoanInterestRate.LoanInterestRateProvided ?? 0,
                LoanInterestRateAnnouncedTypeName = loanInterestRateAnnouncedTypes
                    .FirstOrDefault(t => t.Id == amendmentPriceException.LoanInterestRate.LoanInterestRateAnnouncedType)?
                    .Name ?? string.Empty
            }
        };
    }

    private static WorkflowTaskStates getWorkflowState(_Case.WorkflowTask task)
    {
        if (task.Cancelled)
            return WorkflowTaskStates.Cancelled;

        if (task.StateIdSb == 30 && task.TaskTypeId != (int)WorkflowTaskTypes.PriceException)
            return WorkflowTaskStates.Completed;

        return task.TaskTypeId switch
        {
            1 => getRequestState(task),
            2 => getPriceExceptionState(task),
            3 or 7 => WorkflowTaskStates.Sent,
            6 => getSignatureState(task),
            _ => throw new NobyValidationException($"TaskTypeId {task.TaskTypeId} out of range")
        };
    }

    private static WorkflowTaskStates getRequestState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.ForProcessing,
            2 => WorkflowTaskStates.Sent,
            _ => throw new NobyValidationException($"PhaseTypeId {task.PhaseTypeId} out of range")
        };

    private static WorkflowTaskStates getPriceExceptionState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 when task.StateIdSb == 30 => WorkflowTaskStates.Completed,
            1 => WorkflowTaskStates.Sent,
            2 when task.DecisionId == 1 => WorkflowTaskStates.Schvaleno,
            2 when task.DecisionId == 2 => WorkflowTaskStates.Zamitnuto,
            2 => WorkflowTaskStates.Completed,
            _ => throw new NobyValidationException($"PhaseTypeId {task.PhaseTypeId} out of range")
        };
    
    private static WorkflowTaskStates getSignatureState(_Case.WorkflowTask task) =>
        (SignatureTypes?)task.SignatureTypeId switch
        {
            SignatureTypes.Paper => getPaperSignatureState(task),
            SignatureTypes.Electronic => getDigitalSignatureState(task),
            _ => throw new NobyValidationException($"SignatureTypeId {task.SignatureTypeId} out of range")
        };

    private static WorkflowTaskStates getDigitalSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.ForProcessing,
            2 or 3 => WorkflowTaskStates.Sent,
            _ => throw new NobyValidationException($"PhaseTypeId {task.PhaseTypeId} out of range")
        };

    private static WorkflowTaskStates getPaperSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => WorkflowTaskStates.ForProcessing,
            2 => WorkflowTaskStates.OperationalSupport,
            3 => WorkflowTaskStates.Sent,
            _ => throw new NobyValidationException($"PhaseTypeId {task.PhaseTypeId} out of range")
        };

    private static readonly WorkflowTaskStates[] _amendmentsSigningStates =
    [
        WorkflowTaskStates.Sent,
        WorkflowTaskStates.Completed,
        WorkflowTaskStates.Cancelled
    ];

    private static readonly WorkflowTaskStates[] _amendmentsSigningSignatureLinkStates =
    [
        .. _amendmentsSigningStates,
        WorkflowTaskStates.OperationalSupport
    ];
}