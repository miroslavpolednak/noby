using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.AddDeedOfOwnershipDocument;

internal sealed class AddDeedOfOwnershipDocumentHandler
    : IRequestHandler<AddDeedOfOwnershipDocumentRequest, AddDeedOfOwnershipDocumentResponse>
{
    public async Task<AddDeedOfOwnershipDocumentResponse> Handle(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext
            .RealEstateValuations
            .AnyAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        var entity = new Database.Entities.DeedOfOwnershipDocument
        {
            RealEstateValuationId = request.RealEstateValuationId,
            DeedOfOwnershipId = request.DeedOfOwnershipId,
            Address = request.Address,
            DeedOfOwnershipNumber = request.DeedOfOwnershipNumber,
            CremDeedOfOwnershipDocumentId = request.CremDeedOfOwnershipDocumentId,
            KatuzId = request.KatuzId,
            KatuzTitle = request.KatuzTitle
        };
        if (request.RealEstateIds is not null && request.RealEstateIds.Any())
        {
            entity.RealEstateIds = System.Text.Json.JsonSerializer.Serialize(request.RealEstateIds);
        }
        _dbContext.DeedOfOwnershipDocuments.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AddDeedOfOwnershipDocumentResponse
        {
            DeedOfOwnershipDocumentId = entity.DeedOfOwnershipDocumentId
        };
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public AddDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
