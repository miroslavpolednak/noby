using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetDeedOfOwnershipDocument;

internal sealed class GetDeedOfOwnershipDocumentHandler
    : IRequestHandler<GetDeedOfOwnershipDocumentRequest, DeedOfOwnershipDocument>
{
    public async Task<DeedOfOwnershipDocument> Handle(GetDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.DeedOfOwnershipDocumentId == request.DeedOfOwnershipDocumentId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DeedOfOwnershipDocumentNotFound, request.DeedOfOwnershipDocumentId);

        var response = new DeedOfOwnershipDocument
        {
            DeedOfOwnershipDocumentId = entity.DeedOfOwnershipDocumentId,
            RealEstateValuationId = entity.RealEstateValuationId,
            CremDeedOfOwnershipDocumentId = entity.CremDeedOfOwnershipDocumentId,
            DeedOfOwnershipId = entity.DeedOfOwnershipId,
            Address = entity.Address,
            KatuzTitle = entity.KatuzTitle,
            KatuzId = entity.KatuzId,
            DeedOfOwnershipNumber = entity.DeedOfOwnershipNumber
        };

        if (!string.IsNullOrEmpty(entity.RealEstateIds))
        {
            response.RealEstateIds.AddRange(System.Text.Json.JsonSerializer.Deserialize<long[]>(entity.RealEstateIds));
        }
    
        return response;
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
