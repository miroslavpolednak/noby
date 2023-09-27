using CIS.Core.Exceptions.ExternalServices;
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
        long[]? isActiveRealEstateIds = null;

        if (request.DeedOfOwnershipDocumentId.HasValue)
        {
            var savedDocument = await _realEstateValuation.GetDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId.Value, cancellationToken);
            isActiveRealEstateIds = savedDocument.RealEstateIds?.ToArray();
            documentId = savedDocument.CremDeedOfOwnershipDocumentId;
            deedOfOwnershipNumber = savedDocument.DeedOfOwnershipNumber;
        }
        else
        {
            var foundDocument = (await _cremClient.GetDocuments(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken))
                ?.OrderByDescending(t => t.ValidityDate)
                .FirstOrDefault();

            if (foundDocument is null || foundDocument.PublicDocument || DateTime.Now.Subtract(foundDocument.ValidityDate).TotalDays >= 30)
            {
                try
                {
                    var requestResult = await _cremClient.RequestNewDocumentId(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);

                    documentId = requestResult.CremDeedOfOwnershipDocumentId;
                    deedOfOwnershipNumber = requestResult.DeedOfOwnershipNumber;
                }
                catch (CisExtServiceValidationException ex) when (ex.FirstExceptionCode == "1")
                {
                    throw new NobyValidationException(90035, ex.Message);
                }
                catch (CisException ex) when (ex.ExceptionCode == "404")
                {
                    throw new CisNotFoundException(NobyValidationException.DefaultExceptionCode, "CREM:GetDocuments nothing found");
                }
            }
            else
            {
                documentId = foundDocument.DocumentId;
                deedOfOwnershipNumber = foundDocument.DeedOfOwnershipNumber;
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
                LegalRelationDescription = t.PlainText
            }).ToList(),
            RealEstates = realEstates?.Select(t => new GetDeedOfOwnershipDocumentContentResponseRealEstates
            {
                RealEstateDescription = t.PlainText,
                RealEstateId = t.RealEstateId,
                IsActive = isActiveRealEstateIds?.Contains(t.RealEstateId) ?? false
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
