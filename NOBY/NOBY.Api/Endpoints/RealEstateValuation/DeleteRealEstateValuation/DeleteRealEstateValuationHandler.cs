using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandler
    : IRequestHandler<DeleteRealEstateValuationRequest>
{
    public async Task Handle(DeleteRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        await _realEstateValuationService.DeleteRealEstateValuation(request.CaseId, request.RealEstateValuationId, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public DeleteRealEstateValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
