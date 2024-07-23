using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteDeedOfOwnershipDocument;

internal sealed class DeleteDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<DeleteDeedOfOwnershipDocumentRequest>
{
    public async Task Handle(DeleteDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, false, cancellationToken);
        if (revInstance.PossibleValuationTypeId?.Any() ?? false)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
        }

        await _realEstateValuationService.DeleteDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId, cancellationToken);
    }
}
