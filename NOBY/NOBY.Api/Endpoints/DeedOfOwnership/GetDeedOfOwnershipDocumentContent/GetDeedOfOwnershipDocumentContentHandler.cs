using DomainServices.RealEstateValuationService.Clients;
using ExternalServices.Crem.V1;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

internal sealed class GetDeedOfOwnershipDocumentContentHandler
    : IRequestHandler<GetDeedOfOwnershipDocumentContentRequest, GetDeedOfOwnershipDocumentContentResponse>
{
    public async Task<GetDeedOfOwnershipDocumentContentResponse> Handle(GetDeedOfOwnershipDocumentContentRequest request, CancellationToken cancellationToken)
    {
        long documentId;
        int? deedOfOwnershipNumber = null;

        if (request.DeedOfOwnershipDocumentId.HasValue)
        {
            var savedDocument = await _realEstateValuation.GetDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId.Value, cancellationToken);
            documentId = savedDocument.CremDeedOfOwnershipDocumentId;
            deedOfOwnershipNumber = savedDocument.DeedOfOwnershipNumber;
        }
        else
        {
            var foundDocuments = await _cremClient.GetDocuments(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);

            if (foundDocuments.Count > 1)
            {
                throw new NobyValidationException("CREM:GetDocuments more documents found");
            }

            if (!foundDocuments.Any() || foundDocuments.First().PublicDocument || DateTime.Now.Subtract(foundDocuments.First().ValidityDate).TotalDays >= 30)
            {
                try
                {
                    var requestResult = await _cremClient.RequestNewDocumentId(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);

                    documentId = requestResult.CremDeedOfOwnershipDocumentId;
                    deedOfOwnershipNumber = requestResult.DeedOfOwnershipNumber;
                }
                catch (CisException ex) when (ex.ExceptionCode == "404")
                {
                    throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, "CREM:GetDocuments nothing found");
                }
            }
            else
            {
                documentId = foundDocuments.First().DocumentId;
                deedOfOwnershipNumber = foundDocuments.First().DeedOfOwnershipNumber;
            }
        }

        var legalRelations = await _cremClient.GetLegalRelations(documentId, cancellationToken);
        var realEstates = await _cremClient.GetRealEstates(documentId, cancellationToken);
        var owners = await _cremClient.GetOwners(documentId, cancellationToken);

        return new GetDeedOfOwnershipDocumentContentResponse
        {
            CremDeedOfOwnershipDocumentId = documentId,
            DeedOfOwnershipNumber = deedOfOwnershipNumber,
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
