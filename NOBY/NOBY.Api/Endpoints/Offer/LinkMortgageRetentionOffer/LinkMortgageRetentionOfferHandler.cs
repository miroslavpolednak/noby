using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.OfferLink;
using _SA = DomainServices.SalesArrangementService.Contracts.SalesArrangement;

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
    private readonly MortgageRetentionWorkflowService _retentionWorkflowService;

    public LinkMortgageRetentionOfferHandler(ISalesArrangementServiceClient salesArrangementService, IOfferServiceClient offerService, MortgageRetentionWorkflowService retentionWorkflowService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _retentionWorkflowService = retentionWorkflowService;
    }

    public async Task Handle(LinkMortgageRetentionOfferRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        await _validator.Validate(salesArrangement, offer, cancellationToken);

        var mortgageParameters = new MortgageRetentionParameters(offer.MortgageRetention)
        {
            CaseId = salesArrangement.CaseId,
            TaskProcessId = salesArrangement.TaskProcessId!.Value
        };

        await ProcessWorkflow(mortgageParameters, cancellationToken);

        await UpdateSalesArrangementParameters(request, salesArrangement, cancellationToken);

        await _salesArrangementService.LinkModelationToSalesArrangement(salesArrangement.SalesArrangementId, offer.Data.OfferId, cancellationToken);
    }

    private async Task ProcessWorkflow(IMortgageParameters mortgageParameters, CancellationToken cancellationToken)
    {
        var workflowResult = await _retentionWorkflowService.GetTaskInfoByTaskId(mortgageParameters.CaseId, mortgageParameters.TaskProcessId, cancellationToken);

        await _retentionWorkflowService.UpdateRetentionWorkflowProcess(mortgageParameters, workflowResult.TaskIdSb, cancellationToken);

        await _retentionWorkflowService.CreateIndividualPriceWorkflowTask(workflowResult.TaskList, mortgageParameters, cancellationToken);
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

    private static Task<bool> AdditionalValidation(_SA salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        var result = salesArrangement.CaseId == offer.Data.CaseId && salesArrangement.Retention.ManagedByRC2 != true;

        return Task.FromResult(result);
    }
}