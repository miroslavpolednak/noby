using DomainServices.CodebookService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using _Dto = NOBY.Dto.Workflow;
using CIS.Core.Security;
using CIS.Foms.Enums;
using FastEnumUtility;
using NOBY.Infrastructure.Security;

namespace NOBY.Infrastructure.Services.WorkflowMapper;

[TransientService, AsImplementedInterfacesService]
internal sealed class WorkflowMapperService
    : IWorkflowMapperService
{

    public _Dto.WorkflowProcess MapProcess(_Case.ProcessTask task)
    {
        return new _Dto.WorkflowProcess
        {
            ProcessId = task.ProcessId,
            CreatedOn = task.CreatedOn,
            ProcessNameLong = task.ProcessNameLong,
            StateName = task.StateName,
            ProcessTypeId = task.ProcessTypeId,
            StateIndicator = task.StateIndicator.HasValue ? (_Dto.ProcessStateIndicators)task.StateIndicator : _Dto.ProcessStateIndicators.Unknown //TODO co je default stav?
        };
    }

    public async Task<_Dto.WorkflowTask> MapTask(_Case.WorkflowTask task, CancellationToken cancellationToken = default)
    {
        var taskState = (await _codebookService.WorkflowTaskStatesNoby(cancellationToken))
            .First(s => s.Id == (int)getWorkflowState(task));

        return new()
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
            StateFilter = (_Dto.StateFilters)taskState.Filter,
            StateIndicator = (_Dto.StateIndicators)taskState.Indicator
        };
    }

    public async Task<_Dto.WorkflowTaskDetail> MapTaskDetail(_Case.WorkflowTask task, _Case.TaskDetailItem taskDetailItem, CancellationToken cancellationToken = default)
    {
        return new _Dto.WorkflowTaskDetail
        {
            TaskIdSB = task.TaskIdSb,
            PerformerCode = taskDetailItem.PerformerCode,
            PerformerName = taskDetailItem.PerformanName,
            PerformerLogin = task.PerformerLogin,
            ProcessNameLong = taskDetailItem.ProcessNameLong ?? string.Empty,
            TaskCommunication = taskDetailItem.TaskCommunication?.Select(t => new _Dto.TaskCommunicationItem
            {
                TaskRequest = t.TaskRequest,
                TaskResponse = t.TaskResponse
            }).ToList(),
            Amendments = taskDetailItem.AmendmentsCase switch
            {
                _Case.TaskDetailItem.AmendmentsOneofCase.Request => mapAmendmentsRequest(taskDetailItem.Request),
                _Case.TaskDetailItem.AmendmentsOneofCase.Signing => mapAmendmentsSigning(task, taskDetailItem.Signing),
                _Case.TaskDetailItem.AmendmentsOneofCase.ConsultationData => mapAmendmentsConsultationData(taskDetailItem.ConsultationData),
                _Case.TaskDetailItem.AmendmentsOneofCase.PriceException => await mapAmendmentsPriceException(taskDetailItem.PriceException, cancellationToken),
                _ => null
            }
        };
    }

    private static _Dto.AmendmentsRequest mapAmendmentsRequest(_Case.AmendmentRequest request) => new()
    {
        OrderId = request.OrderId,
        SentToCustomer = request.SentToCustomer
    };

    private _Dto.AmendmentsSigning mapAmendmentsSigning(_Case.WorkflowTask task, _Case.AmendmentSigning signing)
    {
        var stateId = (int)getWorkflowState(task);
        bool remove1 = (new[] { 3, 4, 5 }).Contains(stateId) || !_userAccessor.HasPermission(DomainServices.UserService.Clients.Authorization.UserPermissions.UC_getWflSigningDocuments);

        return new()
        {
            SignatureType = getSignatureType(task),
            Expiration = signing.Expiration,
            FormId = signing.FormId,
            DocumentForSigning = remove1 || stateId == 2 ? "" : signing.DocumentForSigning,
            DocumentForSigningType = signing.DocumentForSigningType,
            ProposalForEntry = remove1 ? "" : (signing.ProposalForEntry == null || !signing.ProposalForEntry.Any() ? "" : signing.ProposalForEntry[0])
        };
    }
    
    private static _Dto.AmendmentsConsultationData mapAmendmentsConsultationData(_Case.AmendmentConsultationData consultationData) => new()
    {
        OrderId = consultationData.OrderId
    };

    private async Task<_Dto.AmendmentsPriceException> mapAmendmentsPriceException(_Case.AmendmentPriceException amendmentPriceException, CancellationToken cancellationToken)
    {
        var decisionTypes = await _codebookService.WorkflowPriceExceptionDecisionTypes(cancellationToken);
        var loanInterestRateAnnouncedTypes = await _codebookService.LoanInterestRateAnnouncedTypes(cancellationToken);
        var fees = await _codebookService.Fees(cancellationToken);

        return new()
        {
            Expiration = amendmentPriceException.Expiration is null ? default(DateOnly?) : amendmentPriceException.Expiration,
            Decision = decisionTypes
            .FirstOrDefault(t => t.Id == amendmentPriceException.DecisionId)?
            .Name ?? string.Empty,
            Fees = amendmentPriceException.Fees
            .Select(t => new _Dto.Fee()
            {
                DiscountPercentage = t.DiscountPercentage,
                FeeName = fees.FirstOrDefault(f => f.Id == Convert.ToInt32(t.FeeId))?.ShortName ?? string.Empty,
                FinalSum = t.FinalSum,
                TariffSum = t.TariffSum
            })
            .ToList(),
            LoanInterestRate = new()
            {
                LoanInterestRate = amendmentPriceException.LoanInterestRate.LoanInterestRate,
                LoanInterestRateDiscount = amendmentPriceException.LoanInterestRate.LoanInterestRateDiscount,
                LoanInterestRateProvided = amendmentPriceException.LoanInterestRate.LoanInterestRateProvided,
                LoanInterestRateAnnouncedTypeName = loanInterestRateAnnouncedTypes
                .FirstOrDefault(t => t.Id == amendmentPriceException.LoanInterestRate.LoanInterestRateAnnouncedType)?
                .Name ?? string.Empty
            }
        };
    }

    private static _Dto.WorkflowTaskStates getWorkflowState(_Case.WorkflowTask task)
    {
        if (task.Cancelled)
            return _Dto.WorkflowTaskStates.Cancelled;

        if (task.StateIdSb == 30)
            return _Dto.WorkflowTaskStates.Completed;

        return task.TaskTypeId switch
        {
            1 => getRequestState(task),
            2 => getPriceExceptionState(task),
            3 or 7 => _Dto.WorkflowTaskStates.Sent,
            6 => getSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static _Dto.WorkflowTaskStates getRequestState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => _Dto.WorkflowTaskStates.ForProcessing,
            2 => _Dto.WorkflowTaskStates.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static _Dto.WorkflowTaskStates getPriceExceptionState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => _Dto.WorkflowTaskStates.Sent,
            2 => _Dto.WorkflowTaskStates.Completed,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static _Dto.SignatureType getSignatureType(_Case.WorkflowTask task) =>
        task.SignatureTypeId switch
        {
            PaperSignatureTypeId => _Dto.SignatureType.Paper,
            DigitalSignatureTypeId => _Dto.SignatureType.Digital,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static _Dto.WorkflowTaskStates getSignatureState(_Case.WorkflowTask task) =>
        task.SignatureTypeId switch
        {
            PaperSignatureTypeId => getPaperSignatureState(task),
            DigitalSignatureTypeId => getDigitalSignatureState(task),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static _Dto.WorkflowTaskStates getDigitalSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => _Dto.WorkflowTaskStates.ForProcessing,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static _Dto.WorkflowTaskStates getPaperSignatureState(_Case.WorkflowTask task) =>
        task.PhaseTypeId switch
        {
            1 => _Dto.WorkflowTaskStates.ForProcessing,
            2 => _Dto.WorkflowTaskStates.OperationalSupport,
            3 => _Dto.WorkflowTaskStates.Sent,
            _ => throw new ArgumentOutOfRangeException()
        };

    private const int PaperSignatureTypeId = 1;
    private const int DigitalSignatureTypeId = 2;
    
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly ICodebookServiceClient _codebookService;

    public WorkflowMapperService(ICodebookServiceClient codebookService, ICurrentUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
        _codebookService = codebookService;
    }
}