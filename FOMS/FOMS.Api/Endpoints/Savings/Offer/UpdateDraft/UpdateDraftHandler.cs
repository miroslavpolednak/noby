using CIS.Core.Results;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class UpdateDraftHandler
    : BaseCaseHandler, IRequestHandler<UpdateDraftRequest, SaveDraftResponse>
{
    public async Task<SaveDraftResponse> Handle(UpdateDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Update SA #{salesArrangementId} for {offerInstanceId}", request.SalesArrangementId, request.OfferInstanceId);

        // get SA instance
        var saInstance = resolveGetSalesArrangementResult(await _aggregate.SalesArrangementService.GetSalesArrangement(request.SalesArrangementId));

        // dotahnout informace o offerInstance
        var offerInstance = await getOfferInstance(request.OfferInstanceId);

        // nalinkovat novou simulaci na SA
        resolveSalesArrangementResult(await _aggregate.SalesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferInstanceId));

        await _aggregate.CaseService.UpdateCaseData(saInstance.CaseId, targetAmount: offerInstance.InputData.TargetAmount);
        //TODO update data klienta na Case?

        return new SaveDraftResponse
        {
            CaseId = saInstance.CaseId,
            SalesArrangementId = request.SalesArrangementId
        };
    }

    private DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse resolveGetSalesArrangementResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse> r => r.Model,
            _ => throw new NotImplementedException()
        };

    private bool resolveSalesArrangementResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            _ => throw new NotImplementedException()
        };

    private readonly ILogger<UpdateDraftHandler> _logger;

    public UpdateDraftHandler(ILogger<UpdateDraftHandler> logger, BaseCaseHandlerAggregate aggregate)
        : base(aggregate)
    {
        _logger = logger;
    }
}
