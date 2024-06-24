using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuation;

internal sealed class DeleteRealEstateValuationHandler(IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<DeleteRealEstateValuationRequest>
{
    public async Task Handle(DeleteRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        await _realEstateValuationService.DeleteRealEstateValuation(request.CaseId, request.RealEstateValuationId, cancellationToken);
    }
}
