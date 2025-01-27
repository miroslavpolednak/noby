﻿using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetDeedOfOwnershipDocuments;

internal sealed class GetDeedOfOwnershipDocumentsHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<GetDeedOfOwnershipDocumentsRequest, GetDeedOfOwnershipDocumentsResponse>
{
    public async Task<GetDeedOfOwnershipDocumentsResponse> Handle(GetDeedOfOwnershipDocumentsRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext
            .RealEstateValuations
            .AnyAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        var entities = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .ToListAsync(cancellationToken);

        var response = new GetDeedOfOwnershipDocumentsResponse();
        response.Documents.AddRange(entities.Select(t =>
        {
            var o = new DeedOfOwnershipDocument
            {
                DeedOfOwnershipDocumentId = t.DeedOfOwnershipDocumentId,
                RealEstateValuationId = t.RealEstateValuationId,
                CremDeedOfOwnershipDocumentId = t.CremDeedOfOwnershipDocumentId,
                DeedOfOwnershipId = t.DeedOfOwnershipId,
                Address = t.Address,
                KatuzTitle = t.KatuzTitle,
                KatuzId = t.KatuzId,
                AddressPointId = t.AddressPointId,
                FlatNumber = t.FlatNumber,
                DeedOfOwnershipNumber = t.DeedOfOwnershipNumber
            };
            if (t.RealEstateIds is not null)
            {
                o.RealEstateIds.AddRange(t.RealEstateIds);
            }
            return o;
        }));

        return response;
    }
}
