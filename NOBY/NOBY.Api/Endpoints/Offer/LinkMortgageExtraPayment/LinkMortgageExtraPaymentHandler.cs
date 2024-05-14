using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.MortgageRefinancing;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageExtraPayment;

internal sealed class LinkMortgageExtraPaymentHandler(
	ICodebookServiceClient _codebookService,
	ICaseServiceClient _caseService,
	ISalesArrangementServiceClient _salesArrangementService,
	ApiServices.MortgageRefinancingSalesArrangementCreateService _salesArrangementCreateService,
	IOfferServiceClient _offerService,
	MortgageRefinancingWorkflowService _refinancingWorkflowService)
		: IRequestHandler<LinkMortgageExtraPaymentRequest, NOBY.Dto.Refinancing.RefinancingLinkResult>
{
    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.MortgageExtraPayment,
        OfferType = OfferTypes.MortgageExtraPayment,
        AdditionalValidation = AdditionalValidation
    };

	public async Task<NOBY.Dto.Refinancing.RefinancingLinkResult> Handle(LinkMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
		// ziskat existujici nebo zalozit novy SA
		var salesArrangement = await _salesArrangementCreateService.GetOrCreateSalesArrangement(request.CaseId, SalesArrangementTypes.MortgageExtraPayment, cancellationToken);

		var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        await ProcessWorkflow(request, offer.MortgageExtraPayment, salesArrangement, cancellationToken);

        await UpdateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(salesArrangement.SalesArrangementId, request.OfferId, cancellationToken);

        return new Dto.Refinancing.RefinancingLinkResult
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ProcessId = salesArrangement.ProcessId
        };
    }

    private async Task ProcessWorkflow(LinkMortgageExtraPaymentRequest request, MortgageExtraPaymentFullData extraPayment, _SA salesArrangement, CancellationToken cancellationToken)
    {
        var workflowResult = await _refinancingWorkflowService.GetProcessInfoByProcessId(request.CaseId, salesArrangement.ProcessId!.Value, cancellationToken);

        await UpdateRetentionWorkflowProcess(extraPayment, salesArrangement.CaseId, workflowResult.ProcessIdSb, cancellationToken);

        var mortgageParameters = new MortgageRefinancingWorkflowParameters
        {
            CaseId = salesArrangement.CaseId,
            ProcessId = salesArrangement.ProcessId!.Value,
            Fee = new MortgageRefinancingWorkflowParameters.FeeObject
            {
                FeeId = await GetFeeId(salesArrangement, cancellationToken),
                FeeSum = extraPayment.SimulationResults.FeeAmount,
                FeeFinalSum = extraPayment.SimulationResults.FeeAmount - ((decimal?)extraPayment.BasicParameters.FeeAmountDiscount ?? 0)
            }
        };

        await _refinancingWorkflowService.CreateIndividualPriceWorkflowTask(mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);
    }

    private async Task UpdateRetentionWorkflowProcess(MortgageExtraPaymentFullData extraPayment, long caseId, int taskIdSb, CancellationToken cancellationToken)
    {
        var updateRequest = new UpdateTaskRequest
        {
            CaseId = caseId,
            TaskIdSb = taskIdSb,
            MortgageExtraPayment = new UpdateTaskRequest.Types.TaskAmendmentMortgageExtraPayment
            {
                ExtraPaymentDate = extraPayment.SimulationInputs.ExtraPaymentDate,
                ExtraPaymentAmount = extraPayment.SimulationResults.ExtraPaymentAmount,
                ExtraPaymentAmountIncludingFee = extraPayment.SimulationResults.ExtraPaymentAmount + extraPayment.SimulationResults.FeeAmount - ((decimal?)extraPayment.BasicParameters.FeeAmountDiscount ?? 0)
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    private Task UpdateSalesArrangementParameters(LinkMortgageExtraPaymentRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.ExtraPayment.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ExtraPayment = salesArrangement.ExtraPayment
        }, cancellationToken);
    }

    private async Task<int> GetFeeId(_SA salesArrangement, CancellationToken cancellationToken)
    {
        var saTypes = await _codebookService.SalesArrangementTypes(cancellationToken);

        return saTypes.First(s => s.Id == salesArrangement.SalesArrangementTypeId).FeeId ?? throw new InvalidOperationException("FeeId of SalesArrangementType codebook is null");
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        return Task.FromResult(salesArrangement.CaseId == offer.Data.CaseId && (offer.Data.Created.DateTime - DateTime.UtcNow).TotalDays < 1);
    }
}