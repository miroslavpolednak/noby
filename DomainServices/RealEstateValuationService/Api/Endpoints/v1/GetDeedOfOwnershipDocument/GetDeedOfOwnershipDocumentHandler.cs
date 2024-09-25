using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.GetDeedOfOwnershipDocument;

internal sealed class GetDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<GetDeedOfOwnershipDocumentRequest, DeedOfOwnershipDocument>
{
    public async Task<DeedOfOwnershipDocument> Handle(GetDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.DeedOfOwnershipDocumentId == request.DeedOfOwnershipDocumentId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.DeedOfOwnershipDocumentNotFound, request.DeedOfOwnershipDocumentId);

        var response = new DeedOfOwnershipDocument
        {
            DeedOfOwnershipDocumentId = entity.DeedOfOwnershipDocumentId,
            RealEstateValuationId = entity.RealEstateValuationId,
            CremDeedOfOwnershipDocumentId = entity.CremDeedOfOwnershipDocumentId,
            DeedOfOwnershipId = entity.DeedOfOwnershipId,
            Address = entity.Address,
            KatuzTitle = entity.KatuzTitle,
            KatuzId = entity.KatuzId,
            AddressPointId = entity.AddressPointId,
            DeedOfOwnershipNumber = entity.DeedOfOwnershipNumber,
            Created = new SharedTypes.GrpcTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime)
        };

        if (entity.RealEstateIds is not null)
        {
            response.RealEstateIds.AddRange(entity.RealEstateIds);
        }

        return response;
    }
}
