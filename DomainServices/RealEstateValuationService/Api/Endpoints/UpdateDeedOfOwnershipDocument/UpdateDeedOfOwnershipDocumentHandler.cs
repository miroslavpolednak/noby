using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateDeedOfOwnershipDocument;

internal sealed class UpdateDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<UpdateDeedOfOwnershipDocumentRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .DeedOfOwnershipDocuments
            .FirstOrDefaultAsync(t => t.DeedOfOwnershipDocumentId == request.DeedOfOwnershipDocumentId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeedOfOwnershipDocumentNotFound, request.DeedOfOwnershipDocumentId);

        entity.RealEstateIds = request.RealEstateIds?.ToList();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
