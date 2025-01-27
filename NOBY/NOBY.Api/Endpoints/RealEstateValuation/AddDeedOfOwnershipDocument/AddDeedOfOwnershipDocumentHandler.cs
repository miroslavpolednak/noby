﻿using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.AddDeedOfOwnershipDocument;

internal sealed class AddDeedOfOwnershipDocumentHandler(IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<RealEstateValuationAddDeedOfOwnershipDocumentRequest, int>
{
    public async Task<int> Handle(RealEstateValuationAddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, false, cancellationToken);
        if (revInstance.PossibleValuationTypeId?.Any() ?? false)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
        }

        if (request.DeedOfOwnershipDocument is null)
        {
            throw new ArgumentNullException(nameof(request), "DeedOfOwnershipDocument is empty");
        }

        if (request.DeedOfOwnershipDocumentDownloaded && (request.DeedOfOwnershipDocument!.RealEstateIds?.Count ?? 0) == 0)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
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
            AddressPointId = request.DeedOfOwnershipDocument.AddressPointId,
            FlatNumber = request.DeedOfOwnershipDocument.FlatNumber
        };
        if (request.DeedOfOwnershipDocument.RealEstateIds is not null)
        {
            dsRequest.RealEstateIds.AddRange(request.DeedOfOwnershipDocument.RealEstateIds);
        }

        return await _realEstateValuationService.AddDeedOfOwnershipDocument(dsRequest, cancellationToken);
    }
}
