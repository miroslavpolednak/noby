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

internal sealed class LinkMortgageExtraPaymentHandler : IRequestHandler<LinkMortgageExtraPaymentRequest>
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly MortgageRefinancingWorkflowService _refinancingWorkflowService;

    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.MortgageExtraPayment,
        OfferType = OfferTypes.MortgageExtraPayment,
        AdditionalValidation = AdditionalValidation
    };

    public LinkMortgageExtraPaymentHandler(
        ICodebookServiceClient codebookService,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        MortgageRefinancingWorkflowService refinancingWorkflowService)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _refinancingWorkflowService = refinancingWorkflowService;
    }

    public async Task Handle(LinkMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        await ProcessWorkflow(request, offer.MortgageExtraPayment, salesArrangement, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);
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
                FeeFinalSum = (decimal?)extraPayment.BasicParameters.FeeAmountDiscounted ?? extraPayment.SimulationResults.FeeAmount
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
                ExtraPaymentAmount = extraPayment.SimulationInputs.ExtraPaymentAmount,
                ExtraPaymentAmountIncludingFee = (decimal?)extraPayment.BasicParameters.FeeAmountDiscounted ?? extraPayment.SimulationResults.FeeAmount
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    private Task UpdateSalesArrangementParameters(LinkMortgageExtraPaymentRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Retention.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            //ExtraPayment
        }, cancellationToken);
    }

    private async Task<int> GetFeeId(_SA salesArrangement, CancellationToken cancellationToken)
    {
        var saTypes = await _codebookService.SalesArrangementTypes(cancellationToken);

        return saTypes.First(s => s.Id == salesArrangement.SalesArrangementTypeId).FeeId ?? throw new InvalidOperationException("FeeId of SalesArrangementType codebook is null");
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        return Task.FromResult(salesArrangement.CaseId == offer.Data.CaseId && (DateTime.UtcNow - offer.Data.Created.DateTime).TotalDays >= 1);
    }
}