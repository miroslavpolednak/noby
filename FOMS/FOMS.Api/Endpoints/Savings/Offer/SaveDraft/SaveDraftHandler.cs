using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class SaveDraftHandler
    : BaseCaseHandler, IRequestHandler<SaveDraftRequest, SaveCaseResponse>
{
    public async Task<SaveCaseResponse> Handle(SaveDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create case for {offerInstanceId}", request.OfferInstanceId);

        // vytvorit case
        long caseId = await createCase(request.OfferInstanceId, request.FirstName, request.LastName, request.DateOfBirth, request.Customer, cancellationToken);
        _logger.LogDebug("Case #{caseId} created", caseId);

        // vytvorit zadost
        int salesArrangementId = await createSalesArrangement(caseId, request.OfferInstanceId, cancellationToken);
        _logger.LogDebug("Sales arrangement #{salesArrangementId} created", salesArrangementId);

        return new SaveCaseResponse
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
