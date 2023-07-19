using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteDeedOfOwnershipDocument;

internal sealed class DeleteDeedOfOwnershipDocumentHandler
    : IRequestHandler<DeleteDeedOfOwnershipDocumentRequest>
{
    public async Task Handle(DeleteDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        // podvrhnute caseId
        if (instance.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
        {
            throw new CisAuthorizationException();
        }

        // spatny stav REV
        if (instance.RealEstateValuationGeneralDetails.ValuationStateId != 7)
        {
            throw new CisAuthorizationException();
        }

        await _realEstateValuationService.DeleteDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public DeleteDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
