using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.Offer.LinkMortgageRetentionOffer;
using NOBY.Services.OfferLink;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

internal sealed class UpdateMortgageRefixationHandler
    : IRequestHandler<UpdateMortgageRefixationRequest>
{
    public async Task Handle(UpdateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        var mortgageParameters = new MortgageRetentionParameters(offer.MortgageRetention)
        {
            CaseId = salesArrangement.CaseId,
            TaskProcessId = salesArrangement.TaskProcessId!.Value
        };

        await ProcessWorkflow(request, mortgageParameters, cancellationToken);

        await UpdateSalesArrangementParameters(request, salesArrangement, cancellationToken);
    }

    private async Task ProcessWorkflow(UpdateMortgageRefixationRequest request, IMortgageParameters mortgageParameters, CancellationToken cancellationToken)
    {
        var workflowResult = await _retentionWorkflowService.GetTaskInfoByTaskId(mortgageParameters.CaseId, mortgageParameters.TaskProcessId, cancellationToken);

        await _retentionWorkflowService.UpdateRetentionWorkflowProcess(mortgageParameters, workflowResult.TaskIdSb, cancellationToken);

        await _retentionWorkflowService.CreateIndividualPriceWorkflowTask(workflowResult.TaskList, mortgageParameters, request.IndividualPriceCommentLastVersion, cancellationToken);
    }

    private Task UpdateSalesArrangementParameters(UpdateMortgageRefixationRequest request, DomainServices.SalesArrangementService.Contracts.SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        salesArrangement.Refixation.IndividualPriceCommentLastVersion = request.IndividualPriceCommentLastVersion;

        return _salesArrangementService.UpdateSalesArrangementParameters(new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Refixation = salesArrangement.Refixation
        }, cancellationToken);
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly MortgageRetentionWorkflowService _retentionWorkflowService;

    public UpdateMortgageRefixationHandler(MortgageRetentionWorkflowService retentionWorkflowService, IOfferServiceClient offerService, ISalesArrangementServiceClient salesArrangementService)
    {
        _retentionWorkflowService = retentionWorkflowService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
    }
}
