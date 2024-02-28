using DomainServices.RealEstateValuationService.Clients;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateDeedOfOwnershipDocument;

public class UpdateDeedOfOwnershipDocumentHandler
    : IRequestHandler<UpdateDeedOfOwnershipDocumentRequest>
{
    public async Task Handle(UpdateDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, false, cancellationToken);
        if (revInstance.PossibleValuationTypeId?.Any() ?? false)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
        }

        await _realEstateValuationService.UpdateDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId, request.RealEstateIds, cancellationToken);
    }
    
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public UpdateDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
