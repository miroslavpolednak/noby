using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateDeedOfOwnershipDocument;

public class UpdateDeedOfOwnershipDocumentHandler
    : IRequestHandler<UpdateDeedOfOwnershipDocumentRequest>
{
    public async Task Handle(UpdateDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        await _realEstateValuationService.UpdateDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId, request.RealEstateIds, cancellationToken);
    }
    
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public UpdateDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
