using DomainServices.CaseService.Contracts;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.MortgageRefinancingWorkflow;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;
using DomainServices.CaseService.Clients.v1;
using NOBY.Services.OfferLink;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageRetentionOffer;

internal class LinkMortgageRetentionOfferHandler : IRequestHandler<LinkMortgageRetentionOfferRequest>
{
    private static readonly MortgageOfferLinkValidator _validator = new()
    {
        SalesArrangementType = SalesArrangementTypes.MortgageRetention,
        OfferType = OfferTypes.MortgageRetention,
        AdditionalValidation = AdditionalValidation
    };

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly MortgageRefinancingWorkflowService _retentionWorkflowService;
    private readonly ICaseServiceClient _caseService;

    public LinkMortgageRetentionOfferHandler(ISalesArrangementServiceClient salesArrangementService, IOfferServiceClient offerService, MortgageRefinancingWorkflowService retentionWorkflowService, ICaseServiceClient caseService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _retentionWorkflowService = retentionWorkflowService;
        _caseService = caseService;
    }

    public async Task Handle(LinkMortgageRetentionOfferRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        await ProcessWorkflow(request, offer.MortgageRetention, salesArrangement, cancellationToken);

        await UpdateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(salesArrangement.SalesArrangementId, offer.Data.OfferId, cancellationToken);
    }

    private async Task ProcessWorkflow(LinkMortgageRetentionOfferRequest request, MortgageRetentionFullData retention, _SA salesArrangement, CancellationToken cancellationToken)
    {
        var workflowResult = await _retentionWorkflowService.GetTaskInfoByTaskId(request.CaseId, salesArrangement.TaskProcessId!.Value, cancellationToken);

        await UpdateRetentionWorkflowProcess(retention, salesArrangement.CaseId, workflowResult.TaskIdSb, cancellationToken);

        var mortgageParameters = new MortgageRefinancingWorkflowParameters
        {
            CaseId = salesArrangement.CaseId,
            TaskProcessId = salesArrangement.TaskProcessId!.Value,
            LoanInterestRate = retention.SimulationInputs.InterestRate,
            LoanInterestRateDiscount = retention.SimulationInputs.InterestRateDiscount,
            Fee = new MortgageRefinancingWorkflowParameters.FeeObject
            {
                FeeSum = retention.BasicParameters.FeeAmount,
                FeeFinalSum = ((decimal?)retention.BasicParameters.FeeAmountDiscounted).GetValueOrDefault()
            }
        };

        await _retentionWorkflowService.CreateIndividualPriceWorkflowTask(workflowResult.TaskList, mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);
    }

    private Task UpdateSalesArrangementParameters(LinkMortgageRetentionOfferRequest request, _SA salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Retention.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Retention = salesArrangement.Retention
        }, cancellationToken);
    }

    private async Task UpdateRetentionWorkflowProcess(MortgageRetentionFullData retention, long caseId, int taskIdSb, CancellationToken cancellationToken)
    {
        var updateRequest = new UpdateTaskRequest
        {
            CaseId = caseId,
            TaskIdSb = taskIdSb,
            Retention = new Retention
            {
                InterestRateValidFrom = retention.SimulationInputs.InterestRateValidFrom,
                LoanInterestRate = (decimal?)retention.SimulationInputs.InterestRate,
                LoanInterestRateProvided = (decimal)retention.SimulationInputs.InterestRate - ((decimal?)retention.SimulationInputs.InterestRateDiscount ?? 0),
                LoanPaymentAmount = retention.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountFinal = retention.SimulationResults.LoanPaymentAmountDiscounted,
                FeeSum = retention.BasicParameters.FeeAmount,
                FeeFinalSum = ((decimal?)retention.BasicParameters.FeeAmountDiscounted).GetValueOrDefault()
            }
        };

        await _caseService.UpdateTask(updateRequest, cancellationToken);
    }

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        var result = salesArrangement.CaseId == offer.Data.CaseId && salesArrangement.Retention.ManagedByRC2 != true;

        return Task.FromResult(result);
    }
}