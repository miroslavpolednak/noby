using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

internal sealed class GetMortgageRetentionHandler
    : IRequestHandler<GetMortgageRetentionRequest, GetMortgageRetentionResponse>
{
    public async Task<GetMortgageRetentionResponse> Handle(GetMortgageRetentionRequest request, CancellationToken cancellationToken)
    {
        // detail retencniho procesu
        var retentionProcess = await getRetentionProcess(request.CaseId, request.ProcessId, cancellationToken);
        
        // zjistit refinancingState
        var (salesArrangement, refinancingState) = await getRefinancingStateId(request.CaseId, retentionProcess, cancellationToken);

        if (refinancingState is (RefinancingStates.Zruseno or RefinancingStates.Dokonceno))
        {
            throw new NobyValidationException(90032, $"RefinancingState is not allowed: {refinancingState}");
        }
        
        // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
        var tasks = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Where(t => t.ProcessId == request.ProcessId)
            .ToList();

        // toto je aktivni task!
        bool existActiveIC = tasks.Any(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled && t.DecisionId != 2 && t.PhaseTypeId == 2);

        var response = new GetMortgageRetentionResponse
        {
            IsReadOnly = refinancingState == RefinancingStates.RozpracovanoVNoby,
            Tasks = (await tasks
                .SelectAsync(t => _workflowMapper.MapTask(t, cancellationToken)))
                .ToList(),
            IndividualPriceCommentLastVersion = salesArrangement?.Retention?.IndividualPriceCommentLastVersion,
            SignatureTypeDetailId = salesArrangement?.Retention?.SignatureTypeDetailId,
            DocumentId = retentionProcess.RefinancingProcess.RefinancingDocumentId,
            RefinancingDocumentEACode = retentionProcess.RefinancingProcess.RefinancingDocumentEACode,
            // doplnit data simulace z procesu (pozdeji mozna prepsat offerou)
            InterestRate = (decimal?)retentionProcess.RefinancingProcess.LoanInterestRate ?? 0M,
            InterestRateDiscount = (decimal?)retentionProcess.RefinancingProcess.LoanInterestRateProvided,
            LoanPaymentAmount = (decimal?)retentionProcess.RefinancingProcess.LoanPaymentAmount ?? 0M,
            LoanPaymentAmountDiscounted = retentionProcess.RefinancingProcess.LoanPaymentAmountFinal,
            FeeAmount = (decimal?)retentionProcess.RefinancingProcess.FeeSum ?? 0M,
            FeeAmountDiscounted = retentionProcess.RefinancingProcess.FeeFinalSum,
            IsGenerateDocumentEnabled = salesArrangement?.OfferId is not null && refinancingState == RefinancingStates.RozpracovanoVNoby && existActiveIC
        };

        // pokud existuje Offer
        if (salesArrangement?.OfferId is not null)
        {
            // detail offer
            var offerInstance = await _offerService.GetOffer(salesArrangement.OfferId.Value, cancellationToken);
            // nacpat data z offer do response misto puvodnich dat z procesu
            replaceTaskDataWithOfferData(response, offerInstance);

            if (existActiveIC)
            {
                response.ContainsInconsistentIndividualPriceData = offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount != response.InterestRateDiscount || offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted != response.FeeAmountDiscounted;

                response.InterestRateDiscount = offerInstance.MortgageRetention.SimulationInputs.InterestRateDiscount;
                response.LoanPaymentAmountDiscounted = offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmountDiscounted;
                response.FeeAmountDiscounted = offerInstance.MortgageRetention.BasicParameters.FeeAmountDiscounted;
            }
        }

        return response;
    }

    /// <summary>
    /// Detail procesu retence
    /// </summary>
    public async Task<DomainServices.CaseService.Contracts.ProcessTask> getRetentionProcess(long caseId, long processId, CancellationToken cancellationToken)
    {
        var process = (await _caseService.GetProcessList(caseId, cancellationToken))
            .FirstOrDefault(p => p.ProcessId == processId)
            ?? throw new NobyValidationException(90043, $"ProccesId not found in list {processId}");

        if (process.ProcessTypeId != 3 || process.RefinancingProcess?.RefinancingType != 1)
        {
            throw new NobyValidationException(90032, "ProcessTypeId!=3 or RefinancingType!=1");
        }

        return process;
    }

    /// <summary>
    /// Vytvoreni detail Offer
    /// </summary>
    private static void replaceTaskDataWithOfferData(GetMortgageRetentionResponse response, GetOfferResponse offerInstance)
    {
        response.FeeAmount = offerInstance.MortgageRetention.BasicParameters.FeeAmount;
        response.InterestRateValidFrom = offerInstance.MortgageRetention.SimulationInputs.InterestRateValidFrom;
        response.InterestRate = offerInstance.MortgageRetention.SimulationInputs.InterestRate;
        response.LoanPaymentAmount = offerInstance.MortgageRetention.SimulationResults.LoanPaymentAmount;
    }

    private async Task<(DomainServices.SalesArrangementService.Contracts.SalesArrangement? salesArrangement, RefinancingStates RefinancingState)> getRefinancingStateId(long caseId, DomainServices.CaseService.Contracts.ProcessTask process, CancellationToken cancellationToken)
    {
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var currentProcessSA = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.TaskProcessId == process.ProcessId);
        if (currentProcessSA is not null)
        {
            var currentProcessSADetail = await _salesArrangementService.GetSalesArrangement(currentProcessSA.SalesArrangementId, cancellationToken);
            if (currentProcessSA.Retention?.ManagedByRC2 ?? false)
            {
                // ref.state staci vzit pouze z SA
                return (currentProcessSADetail, RefinancingHelper.GetRefinancingState((SalesArrangementStates)currentProcessSA.State));
            }
        }

        return (null, RefinancingHelper.GetRefinancingState(false, currentProcessSA?.TaskProcessId, process));
    }

    private readonly Services.WorkflowMapper.IWorkflowMapperService _workflowMapper;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public GetMortgageRetentionHandler(ISalesArrangementServiceClient salesArrangementService, ICaseServiceClient caseService, IOfferServiceClient offerService, Services.WorkflowMapper.IWorkflowMapperService workflowMapper)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _offerService = offerService;
        _workflowMapper = workflowMapper;
    }
}
