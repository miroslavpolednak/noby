using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.AddDeedOfOwnershipDocument;

internal sealed class AddDeedOfOwnershipDocumentHandler
    : IRequestHandler<AddDeedOfOwnershipDocumentRequest, int>
{
    public async Task<int> Handle(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.AddDeedOfOwnershipDocumentRequest
        {
            Address = request.Address,
            CremDeedOfOwnershipDocumentId = request.CremDeedOfOwnershipDocumentId,
            DeedOfOwnershipId = request.DeedOfOwnershipId,
            DeedOfOwnershipNumber = request.DeedOfOwnershipNumber,
            KatuzId = request.KatuzId,
            KatuzTitle = request.KatuzTitle,
            RealEstateValuationId = request.RealEstateValuationId,
        };
        if (request.RealEstateIds is not null)
        {
            dsRequest.RealEstateIds.AddRange(request.RealEstateIds);
        }

        return await _realEstateValuationService.AddDeedOfOwnershipDocument(dsRequest, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public AddDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
