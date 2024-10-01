using CIS.Core.Exceptions.ExternalServices;
using DomainServices.RealEstateValuationService.Clients;
using ExternalServices.Crem.V1;
using ExternalServices.Crem.V1.Contracts;

namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

internal sealed class GetDeedOfOwnershipDocumentContentHandler(
    TimeProvider _timeProvider,
    ICremClient _cremClient, 
    IRealEstateValuationServiceClient _realEstateValuation)
        : IRequestHandler<GetDeedOfOwnershipDocumentContentRequest, DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse>
{
    public async Task<DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse> Handle(GetDeedOfOwnershipDocumentContentRequest request, CancellationToken cancellationToken)
    {
        DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse response = new();
        long[]? isActiveRealEstateIds = null;

        if (request.DeedOfOwnershipDocumentId.HasValue)
        {
            var savedDocument = await _realEstateValuation.GetDeedOfOwnershipDocument(request.DeedOfOwnershipDocumentId.Value, cancellationToken);
            isActiveRealEstateIds = savedDocument.RealEstateIds?.ToArray();
            response.CremDeedOfOwnershipDocumentId = savedDocument.CremDeedOfOwnershipDocumentId;
            response.DeedOfOwnershipNumber = savedDocument.DeedOfOwnershipNumber;

            if ((savedDocument.RealEstateIds?.Count ?? 0) > 0)
            {
                response.DeedOfOwnershipDocumentId = savedDocument.DeedOfOwnershipDocumentId;
                response.DeedOfOwnershipDocumentDownloaded = true;
            }
            // entita je starsi nez 23 hodin
            else if (savedDocument.Created.DateTime < _timeProvider.GetLocalNow().AddHours(-23).DateTime)
            {
                await _realEstateValuation.DeleteDeedOfOwnershipDocument(savedDocument.DeedOfOwnershipDocumentId, cancellationToken);

                var result = await _cremClient.RequestNewDocumentId(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);

                response.CremDeedOfOwnershipDocumentId = result.CremDeedOfOwnershipDocumentId;
                response.DeedOfOwnershipNumber = result.DeedOfOwnershipNumber;
                response.DeedOfOwnershipDocumentDownloaded = await checkIfDownloaded(result.CremDeedOfOwnershipDocumentId, cancellationToken);
            }
            else // ulozena entita je mladsi nez 23 hodin
            {
                response.DeedOfOwnershipDocumentId = savedDocument.DeedOfOwnershipDocumentId;
                response.DeedOfOwnershipDocumentDownloaded = await checkIfDownloaded(savedDocument.CremDeedOfOwnershipDocumentId, cancellationToken);
            }
        }
        else if (request.CremDeedOfOwnershipDocumentId.HasValue)
        {
            response.DeedOfOwnershipDocumentDownloaded = await checkIfDownloaded(request.CremDeedOfOwnershipDocumentId.Value, cancellationToken);
        }
        else
        {
            var foundDocument = (await _cremClient.GetDocuments(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken))
                ?.OrderByDescending(t => t.ValidityDate)
                .FirstOrDefault();

            if (foundDocument is null 
                || foundDocument.PublicDocument 
                || DateTime.Now.Subtract(foundDocument.ValidityDate).TotalDays >= 30 
                || foundDocument.Status != ExternalServices.Crem.V1.Contracts.DeedOfOwnershipDocumentStatus.INVOICED)
            {
                try
                {
                    var result = await _cremClient.RequestNewDocumentId(request.KatuzId, request.DeedOfOwnershipNumber, request.DeedOfOwnershipId, cancellationToken);

                    response.CremDeedOfOwnershipDocumentId = result.CremDeedOfOwnershipDocumentId;
                    response.DeedOfOwnershipNumber = result.DeedOfOwnershipNumber;
                    response.DeedOfOwnershipDocumentDownloaded = await checkIfDownloaded(result.CremDeedOfOwnershipDocumentId, cancellationToken);
                }
                catch (CisExternalServiceValidationException ex) when (ex.FirstExceptionCode == "404")
                {
                    throw new CisNotFoundException(90047, "");
                }
            }
            else
            {
                response.DeedOfOwnershipDocumentDownloaded = true;
                response.CremDeedOfOwnershipDocumentId = foundDocument.DocumentId;
                response.DeedOfOwnershipNumber = foundDocument.DeedOfOwnershipNumber;
            }
        }

        if (response.DeedOfOwnershipDocumentDownloaded)
        {
            var (estates, firstDeedOfOwnershipNumber) = await getRealEstates(response.CremDeedOfOwnershipDocumentId, isActiveRealEstateIds, cancellationToken);
            response.RealEstates = estates;
            if (response.DeedOfOwnershipNumber.GetValueOrDefault() == 0 && firstDeedOfOwnershipNumber.HasValue)
            {
                response.DeedOfOwnershipNumber = firstDeedOfOwnershipNumber;
            }
            response.Owners = await getOwners(response.CremDeedOfOwnershipDocumentId, cancellationToken);

            var legalRelations = await _cremClient.GetLegalRelations(response.CremDeedOfOwnershipDocumentId, cancellationToken);

            response.LegalRelationsSectionB1 = getLegalRelations(legalRelations, DeedOfOwnershipLegalRelationSection.B1);
            response.LegalRelationsSectionC = getLegalRelations(legalRelations, DeedOfOwnershipLegalRelationSection.C);
            response.LegalRelationsSectionD = getLegalRelations(legalRelations, DeedOfOwnershipLegalRelationSection.D);
        }

        return response;
    }
    
    private async Task<bool> checkIfDownloaded(long cremDeedOfOwnershipDocumentId, CancellationToken cancellationToken)
    {
        return await _cremClient.TryToConfirmDownload(cremDeedOfOwnershipDocumentId, cancellationToken);
    }

    private async Task<(List<DeedOfOwnershipGetDeedOfOwnershipDocumentContentRealEstates>? Estates, int? FirstDeedOfOwnershipNumber)> getRealEstates(long cremDeedOfOwnershipDocumentId, long[]? isActiveRealEstateIds, CancellationToken cancellationToken)
    {
        var realEstates = await _cremClient.GetRealEstates(cremDeedOfOwnershipDocumentId, cancellationToken);

        var estates = realEstates?.Select(t => new DeedOfOwnershipGetDeedOfOwnershipDocumentContentRealEstates
        {
            RealEstateDescription = t.PlainText,
            RealEstateId = t.RealEstateId,
            IsActive = isActiveRealEstateIds?.Contains(t.RealEstateId) ?? false
        }).ToList();

        return (estates, realEstates?.FirstOrDefault()?.DeedOfOwnershipNumber);
    }

    private static List<DeedOfOwnershipGetDeedOfOwnershipDocumentContentLegalRelation>? getLegalRelations(ICollection<DeedOfOwnershipLegalRelation>? legalRelations, DeedOfOwnershipLegalRelationSection section)
    {
        return legalRelations?
            .Where(t => t.Section == section)
            .Select(t => new DeedOfOwnershipGetDeedOfOwnershipDocumentContentLegalRelation
            {
                LegalRelationDescription = t.PlainText
            })
            .ToList();
    }

    private async Task<List<DeedOfOwnershipGetDeedOfOwnershipDocumentContentOwners>?> getOwners(long cremDeedOfOwnershipDocumentId, CancellationToken cancellationToken)
    {
        var owners = await _cremClient.GetOwners(cremDeedOfOwnershipDocumentId, cancellationToken);

        return owners?.Select(t => new DeedOfOwnershipGetDeedOfOwnershipDocumentContentOwners
        {
            OwnerDescription = t.Description,
            OwnershipRatio = t.OwnershipRatio
        }).ToList();
    }
}
