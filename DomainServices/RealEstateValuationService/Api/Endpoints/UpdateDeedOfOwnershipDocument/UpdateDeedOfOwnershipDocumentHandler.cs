using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateDeedOfOwnershipDocument;

internal sealed class UpdateDeedOfOwnershipDocumentHandler
    : IRequestHandler<UpdateDeedOfOwnershipDocumentRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .DeedOfOwnershipDocuments
            .FirstOrDefaultAsync(t => t.DeedOfOwnershipDocumentId == request.DeedOfOwnershipDocumentId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeedOfOwnershipDocumentNotFound, request.DeedOfOwnershipDocumentId);

        if (request.RealEstateIds is not null && request.RealEstateIds.Any())
        {
            entity.RealEstateIds = System.Text.Json.JsonSerializer.Serialize(request.RealEstateIds);
        }
        else
        {
            entity.RealEstateIds = null;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public UpdateDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
