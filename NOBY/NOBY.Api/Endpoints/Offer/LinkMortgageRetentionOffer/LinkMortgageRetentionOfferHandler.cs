using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using NOBY.Services.MortgageRefinancing;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageRetentionOffer;

internal sealed class LinkMortgageRetentionOfferHandler(
	ICodebookServiceClient _codebookService,
	ISalesArrangementServiceClient _salesArrangementService,
	ApiServices.MortgageRefinancingSalesArrangementCreateService _salesArrangementCreateService,
	IOfferServiceClient _offerService,
	MortgageRefinancingWorkflowService _refinancingWorkflowService,
	ICaseServiceClient _caseService) 
    : IRequestHandler<OfferLinkMortgageRetentionOfferRequest, OfferRefinancingLinkResult>
{
    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.MortgageRetention,
        OfferType = OfferTypes.MortgageRetention,
        AdditionalValidation = AdditionalValidation
    };

	public async Task<OfferRefinancingLinkResult> Handle(OfferLinkMortgageRetentionOfferRequest request, CancellationToken cancellationToken)
    {
		// ziskat existujici nebo zalozit novy SA
		var salesArrangement = await _salesArrangementCreateService.GetOrCreateSalesArrangement(request.CaseId, SalesArrangementTypes.MortgageRetention, cancellationToken);

        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        _refinancingWorkflowService.ValidateIndividualPriceExceptionComment(request.IndividualPriceCommentLastVersion, offer.MortgageRetention.SimulationInputs.InterestRateDiscount, offer.MortgageRetention.BasicParameters.FeeAmountDiscounted);

        await ProcessWorkflow(request, offer.MortgageRetention, salesArrangement, cancellationToken);

        await UpdateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(salesArrangement.SalesArrangementId, offer.Data.OfferId, cancellationToken);

        return new()
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ProcessId = salesArrangement.ProcessId
        };
	}

    private async Task ProcessWorkflow(OfferLinkMortgageRetentionOfferRequest request, MortgageRetentionFullData retention, _SA salesArrangement, CancellationToken cancellationToken)
    {
        var workflowResult = await _refinancingWorkflowService.GetProcessInfoByProcessId(request.CaseId, salesArrangement.ProcessId!.Value, cancellationToken);

        await UpdateRetentionWorkflowProcess(retention, salesArrangement.CaseId, workflowResult.ProcessIdSb, cancellationToken);

        var mortgageParameters = new MortgageRefinancingWorkflowParameters
        {
            CaseId = salesArrangement.CaseId,
            ProcessId = salesArrangement.ProcessId!.Value,
            LoanInterestRate = retention.SimulationInputs.InterestRate,
            LoanInterestRateDiscount = retention.SimulationInputs.InterestRateDiscount,
            Fee = new MortgageRefinancingWorkflowParameters.FeeObject
            {
                FeeId = await GetFeeId(salesArrangement, cancellationToken),
                FeeSum = retention.BasicParameters.FeeAmount,
                FeeFinalSum = (decimal?)retention.BasicParameters.FeeAmountDiscounted ?? retention.BasicParameters.FeeAmount
            }
        };

        await _refinancingWorkflowService.CreateIndividualPriceWorkflowTask(mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);
    }

    private async Task UpdateRetentionWorkflowProcess(MortgageRetentionFullData retention, long caseId, int taskIdSb, CancellationToken cancellationToken)
    {
        var updateRequest = new UpdateTaskRequest
        {
            CaseId = caseId,
            TaskIdSb = taskIdSb,
            MortgageRetention = new UpdateTaskRequest.Types.TaskAmendmentMortgageRetention
            {
                InterestRateValidFrom = retention.SimulationInputs.InterestRateValidFrom,
                LoanInterestRate = (decimal?)retention.SimulationInputs.InterestRate,
                LoanInterestRateProvided = (decimal)retention.SimulationInputs.InterestRate - ((decimal?)retention.SimulationInputs.InterestRateDiscount ?? 0),
                LoanPaymentAmount = retention.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountFinal = (decimal?)retention.SimulationResults.LoanPaymentAmountDiscounted ?? retention.SimulationResults.LoanPaymentAmount,
                FeeSum = retention.BasicParameters.FeeAmount,
                FeeFinalSum = (decimal?)retention.BasicParameters.FeeAmountDiscounted ?? retention.BasicParameters.FeeAmount
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    private Task UpdateSalesArrangementParameters(OfferLinkMortgageRetentionOfferRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Retention.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Retention = salesArrangement.Retention
        }, cancellationToken);
    }

    private async Task<int> GetFeeId(_SA salesArrangement, CancellationToken cancellationToken)
    {
        var saTypes = await _codebookService.SalesArrangementTypes(cancellationToken);

        return saTypes.First(s => s.Id == salesArrangement.SalesArrangementTypeId).FeeId ?? throw new InvalidOperationException("FeeId of SalesArrangementType codebook is null");
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        var result = salesArrangement.CaseId == offer.Data.CaseId && salesArrangement.Retention.ManagedByRC2 != true;

        return Task.FromResult(result);
    }
}