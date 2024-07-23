using CIS.Core.Exceptions.ExternalServices;
using DomainServices.RealEstateValuationService.Clients;
using ExternalServices.Crem.V1;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

internal sealed class GetDeedOfOwnershipDocumentContentHandler(
    ICremClient _cremClient, 
    IRealEstateValuationServiceClient _realEstateValuation)
        : IRequestHandler<GetDeedOfOwnershipDocumentContentRequest, DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse>
{
    public async Task<DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse> Handle(GetDeedOfOwnershipDocumentContentRequest request, CancellationToken cancellationToken)
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
                catch (CisExternalServiceValidationException ex) when (ex.FirstExceptionCode == "1")
                {
                    throw new NobyValidationException(90035, ex.Message);
                }
                catch (CisExternalServiceValidationException ex) when (ex.FirstExceptionCode == "404")
                {
                    throw new CisNotFoundException(90047, "");
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
        
        return new DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse
        {
            CremDeedOfOwnershipDocumentId = documentId,
            DeedOfOwnershipNumber = deedOfOwnershipNumber.GetValueOrDefault() == 0 ? realEstates.FirstOrDefault()?.DeedOfOwnershipNumber : deedOfOwnershipNumber,
            Owners = owners?.Select(t => new DeedOfOwnershipGetDeedOfOwnershipDocumentContentOwners
            {
                OwnerDescription = t.Description,
                OwnershipRatio = t.OwnershipRatio
            }).ToList(),
            LegalRelations = legalRelations?.Select(t => new DeedOfOwnershipGetDeedOfOwnershipDocumentContentLegalRelations
            {
                LegalRelationDescription = t.PlainText
            }).ToList(),
            RealEstates = realEstates?.Select(t => new DeedOfOwnershipGetDeedOfOwnershipDocumentContentRealEstates
            {
                RealEstateDescription = t.PlainText,
                RealEstateId = t.RealEstateId,
                IsActive = isActiveRealEstateIds?.Contains(t.RealEstateId) ?? false
            }).ToList()
        };
    }
}
