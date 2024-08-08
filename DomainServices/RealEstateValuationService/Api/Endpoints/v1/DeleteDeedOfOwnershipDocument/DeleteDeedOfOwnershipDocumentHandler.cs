using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.DeleteDeedOfOwnershipDocument;

internal sealed class DeleteDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<DeleteDeedOfOwnershipDocumentRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .DeedOfOwnershipDocuments
            .FirstOrDefaultAsync(t => t.DeedOfOwnershipDocumentId == request.DeedOfOwnershipDocumentId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.DeedOfOwnershipDocumentNotFound, request.DeedOfOwnershipDocumentId);

        _dbContext.DeedOfOwnershipDocuments.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
