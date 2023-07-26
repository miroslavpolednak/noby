using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.AddDeedOfOwnershipDocument;

internal sealed class AddDeedOfOwnershipDocumentHandler
    : IRequestHandler<AddDeedOfOwnershipDocumentRequest, int>
{
    public async Task<int> Handle(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        if (request.DeedOfOwnershipDocument is null)
        {
            throw new ArgumentNullException(nameof(request), "DeedOfOwnershipDocument is empty");
        }

        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.AddDeedOfOwnershipDocumentRequest
        {
            Address = request.DeedOfOwnershipDocument.Address,
            CremDeedOfOwnershipDocumentId = request.DeedOfOwnershipDocument.CremDeedOfOwnershipDocumentId,
            DeedOfOwnershipId = request.DeedOfOwnershipDocument.DeedOfOwnershipId,
            DeedOfOwnershipNumber = request.DeedOfOwnershipDocument.DeedOfOwnershipNumber,
            KatuzId = request.DeedOfOwnershipDocument.KatuzId,
            KatuzTitle = request.DeedOfOwnershipDocument.KatuzTitle,
            RealEstateValuationId = request.RealEstateValuationId,
            AddressPointId = request.DeedOfOwnershipDocument.AddressPointId
        };
        if (request.DeedOfOwnershipDocument.RealEstateIds is not null)
        {
            dsRequest.RealEstateIds.AddRange(request.DeedOfOwnershipDocument.RealEstateIds);
        }

        return await _realEstateValuationService.AddDeedOfOwnershipDocument(dsRequest, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public AddDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
