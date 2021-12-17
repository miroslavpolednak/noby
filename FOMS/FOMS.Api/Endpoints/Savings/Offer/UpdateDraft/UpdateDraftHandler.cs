using CIS.Core.Results;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer;

internal class UpdateDraftHandler
    : IRequestHandler<UpdateDraftRequest, SaveDraftResponse>
{
    public async Task<SaveDraftResponse> Handle(UpdateDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Update SA #{salesArrangementId} for {offerInstanceId}", request.SalesArrangementId, request.OfferInstanceId);

        // nalinkovat novou simulaci na SA
        resolveSalesArrangementResult(await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferInstanceId));
        
        return new SaveDraftResponse
        {
            SalesArrangementId = request.SalesArrangementId
        };
    }

    private bool resolveSalesArrangementResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<SaveDraftHandler> _logger;

    public UpdateDraftHandler(
        ILogger<SaveDraftHandler> logger,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
