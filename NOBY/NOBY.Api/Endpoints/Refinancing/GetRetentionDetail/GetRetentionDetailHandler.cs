using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetRetentionDetail;

internal sealed class GetRetentionDetailHandler
    : IRequestHandler<GetRetentionDetailRequest, GetRetentionDetailResponse>
{
    public async Task<GetRetentionDetailResponse> Handle(GetRetentionDetailRequest request, CancellationToken cancellationToken)
    {
        // zjistit refinancingState
        var (offerId, refinancingState) = await getRefinancingStateId(request.CaseId, request.ProcessId, cancellationToken);

        if (refinancingState is (RefinancingStates.Zruseno or RefinancingStates.Dokonceno))
        {
            throw new NobyValidationException(90032, $"RefinancingState is not allowed: {refinancingState}");
        }

        // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
        var tasks = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Where(t => t.ProcessId == request.ProcessId)
            .ToList();

        // pro IC potrebujeme i detail
        var icTask = tasks.FirstOrDefault(t => !t.Cancelled && t.TaskTypeId == (int)WorkflowTaskTypes.PriceException);

        var response = new GetRetentionDetailResponse
        {
            IsReadonly = refinancingState == RefinancingStates.RozpracovanoVNoby,
            OtherWorkflowTasks = (await tasks
                .Where(t => t.TaskId != icTask?.TaskId)
                .SelectAsync(t => _workflowMapper.MapTask(t, cancellationToken)))
                .ToList(),
            CurrentPriceExceptionTask = await getIC(icTask, cancellationToken),
            ICkomentar = "!!! TODO !!!"
        };

        // pokud existuje Offer
        if (offerId.HasValue)
        {
            // detail offer
            response.Offer = await getOffer(offerId.Value, cancellationToken);

            // jestlize neexistuje IC, ujisti se, ze na Offer neni zadna informace o IC
            if (response.CurrentPriceExceptionTask is null)
            {
                response.Offer.InterestRateDiscount = null;
                response.Offer.LoanPaymentAmountDiscounted = null;
            }
            // jestlize se nalezena IC neshoduje s parametry na Offer
            else if (response.Offer.InterestRateDiscount != response.CurrentPriceExceptionTask?.PriceExceptionDetails?.LoanInterestRate?.LoanInterestRateDiscount
                || response.Offer.FeeAmountDiscounted != response.CurrentPriceExceptionTask?.PriceExceptionDetails?.Fees?.FirstOrDefault()?.FinalSum)
            {
                response.ContainsInconsistentIndividualPriceData = true;
            }
        }

        return response;
    }

    /// <summary>
    /// Vytvoreni detailu IC
    /// </summary>
    private async Task<Dto.GetRetentionDetailPriceExceptionTask?> getIC(DomainServices.CaseService.Contracts.WorkflowTask? task, CancellationToken cancellationToken)
    {
        if (task is null)
        {
            return null;
        }

        var taskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);
        var mappedTaskDetail = await _workflowMapper.MapTaskDetail(task, taskDetail.TaskDetail, cancellationToken);

        return new Dto.GetRetentionDetailPriceExceptionTask
        {
            Task = await _workflowMapper.MapTask(task, cancellationToken),
            PriceExceptionDetails = mappedTaskDetail.Amendments as NOBY.Dto.Workflow.AmendmentsPriceException
        };
    }

    /// <summary>
    /// Vytvoreni detail Offer
    /// </summary>
    private async Task<Dto.GetRetentionDetailOffer> getOffer(int offerId, CancellationToken cancellationToken)
    {
        var offerInstance = await _offerService.GetOffer(offerId, cancellationToken);

        return new Dto.GetRetentionDetailOffer
        {
            FeeAmountDiscounted = offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted,
            FeeAmount = offerInstance.MortgageRetention.BasicParameters.FeeAmount,
            InterestRateValidFrom = offerInstance.MortgageRetention.SimulationInputs.InterestRateValidFrom,
            InterestRate = offerInstance.MortgageRetention.SimulationInputs.InterestRate,
            InterestRateDiscount = offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount,
            LoanPaymentAmount = offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmountDiscounted,
            OfferId = offerId
        };
    }

    private async Task<(int? OfferId, RefinancingStates RefinancingState)> getRefinancingStateId(long caseId, long processId, CancellationToken cancellationToken)
    {
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var currentProcessSA = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.TaskProcessId == processId);
        if (currentProcessSA is not null)
        {
            var currentProcessSADetail = await _salesArrangementService.GetSalesArrangement(currentProcessSA.SalesArrangementId, cancellationToken);
            if (currentProcessSA.Retention?.ManagedByRC2 ?? false)
            {
                // ref.state staci vzit pouze z SA
                return (currentProcessSADetail.OfferId, RefinancingHelper.GetRefinancingState((SalesArrangementStates)currentProcessSA.State));
            }
        }

        // ziskat detail procesu
        var processes = await _caseService.GetProcessList(caseId, cancellationToken);
        var process = processes.FirstOrDefault(t => t.ProcessId == processId)
            ?? throw new NobyValidationException($"ProcessId {processId} not found in GetProcessList");

        if (process.ProcessTypeId != 3 || process.RefinancingProcess?.RefinancingType != 1)
        {
            throw new NobyValidationException(90032, "ProcessTypeId!=3 or RefinancingType!=1");
        }

        return (null, RefinancingHelper.GetRefinancingState(currentProcessSA?.Retention?.ManagedByRC2 ?? false, currentProcessSA?.TaskProcessId, process));
    }

    private readonly Services.WorkflowMapper.IWorkflowMapperService _workflowMapper;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public GetRetentionDetailHandler(ISalesArrangementServiceClient salesArrangementService, ICaseServiceClient caseService, IOfferServiceClient offerService, Services.WorkflowMapper.IWorkflowMapperService workflowMapper)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _offerService = offerService;
        _workflowMapper = workflowMapper;
    }
}
