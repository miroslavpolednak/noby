using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateDeedOfOwnershipDocument;

public class UpdateDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<RealEstateValuationUpdateDeedOfOwnershipDocumentRequest>
{
    public async Task Handle(RealEstateValuationUpdateDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, false, cancellationToken);
        if (revInstance.PossibleValuationTypeId?.Any() ?? false)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
        }

        await _realEstateValuationService.UpdateDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId, request.RealEstateIds, cancellationToken);
    }
}
