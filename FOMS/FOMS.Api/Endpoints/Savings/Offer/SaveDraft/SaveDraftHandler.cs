using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class SaveDraftHandler
    : BaseCaseHandler, IRequestHandler<SaveDraftRequest, SaveDraftResponse>
{
    public async Task<SaveDraftResponse> Handle(SaveDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create case for {offerInstanceId}", request.OfferInstanceId);

        // vytvorit case
        long caseId = await createCase(request.OfferInstanceId, request.FirstName, request.LastName, request.DateOfBirth, request.Customer);
        _logger.LogDebug("Case #{caseId} created", caseId);

        // vytvorit zadost
        int salesArrangementId = await createSalesArrangement(caseId, request.OfferInstanceId);
        _logger.LogDebug("Sales arrangement #{salesArrangementId} created", salesArrangementId);

        return new SaveDraftResponse
        {
            CaseId = caseId,
            SalesArrangementId = salesArrangementId
        };
    }

    private readonly ILogger<SaveDraftHandler> _logger;

    public SaveDraftHandler(ILogger<SaveDraftHandler> logger, BaseCaseHandlerAggregate aggregate)
        : base(aggregate)
    {
        _logger = logger;
    }
}
