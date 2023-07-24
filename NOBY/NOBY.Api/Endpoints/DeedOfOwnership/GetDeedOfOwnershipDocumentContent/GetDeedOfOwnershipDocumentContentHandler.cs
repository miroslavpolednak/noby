using DomainServices.RealEstateValuationService.Clients;
using ExternalServices.Crem.V1;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

internal sealed class GetDeedOfOwnershipDocumentContentHandler
    : IRequestHandler<GetDeedOfOwnershipDocumentContentRequest, GetDeedOfOwnershipDocumentContentResponse>
{
    public async Task<GetDeedOfOwnershipDocumentContentResponse> Handle(GetDeedOfOwnershipDocumentContentRequest request, CancellationToken cancellationToken)
    {
        long documentId;

        if (request.NobyDeedOfOwnershipDocumentId.HasValue)
        {
            documentId = request.NobyDeedOfOwnershipDocumentId.Value;
        }
        else
        {
            var foundDocuments = await _cremClient.GetDocuments(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);

            if (foundDocuments is null || !foundDocuments.Any())
            {
                throw new NobyValidationException("CREM:GetDocuments nothing found");
            }
            else if (foundDocuments.Count > 1)
            {
                throw new NobyValidationException("CREM:GetDocuments more documents found");
            }

            if (!foundDocuments.First().PublicDocument && DateTime.Now.Subtract(foundDocuments.First().ValidityDate).TotalDays <= 30)
            {
                documentId = request.DeedOfOwnershipId.GetValueOrDefault();
            }
            else
            {
                documentId = await _cremClient.RequestNewDocumentId(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);
            }
        }

        var legalRelations = await _cremClient.GetLegalRelations(documentId, cancellationToken);
        var realEstates = await _cremClient.GetRealEstates(documentId, cancellationToken);
        var owners = await _cremClient.GetOwners(documentId, cancellationToken);

        return new GetDeedOfOwnershipDocumentContentResponse
        {
            Owners = owners?.Select(t => new GetDeedOfOwnershipDocumentContentResponseOwners
            {
                OwnerDescription = t.Description,
                OwnershipRatio = t.OwnershipRatio
            }).ToList(),
            LegalRelations = legalRelations?.Select(t => new GetDeedOfOwnershipDocumentContentResponseLegalRelations
            {
                LegalRelationDescription = t.LegalRelationDesc
            }).ToList(),
            RealEstates = realEstates?.Select(t => new GetDeedOfOwnershipDocumentContentResponseRealEstates
            {
                RealEstateDescription = t.PlainText,
                RealEstateId = t.RealEstateId,
                IsActive = false
            }).ToList()
        };
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuation;
    private readonly ICremClient _cremClient;

    public GetDeedOfOwnershipDocumentContentHandler(ICremClient cremClient, IRealEstateValuationServiceClient realEstateValuation)
    {
        _realEstateValuation = realEstateValuation;
        _cremClient = cremClient;
    }
}
