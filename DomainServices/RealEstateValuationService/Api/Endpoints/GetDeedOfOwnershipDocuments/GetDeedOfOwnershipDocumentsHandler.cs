using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetDeedOfOwnershipDocuments;

internal sealed class GetDeedOfOwnershipDocumentsHandler
    : IRequestHandler<GetDeedOfOwnershipDocumentsRequest, GetDeedOfOwnershipDocumentsResponse>
{
    public async Task<GetDeedOfOwnershipDocumentsResponse> Handle(GetDeedOfOwnershipDocumentsRequest request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext
            .RealEstateValuations
            .AnyAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        var entities = await _dbContext.DeedOfOwnershipDocuments
            .AsNoTracking()
            .Where(t => t.RealEstateValuationId == request.RealEstateValuationId)
            .ToListAsync(cancellationToken);

        var response = new GetDeedOfOwnershipDocumentsResponse();
        response.Documents.AddRange(entities.Select(t => {
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
                DeedOfOwnershipNumber = t.DeedOfOwnershipNumber
            };
            if (!string.IsNullOrEmpty(t.RealEstateIds))
            {
                o.RealEstateIds.AddRange(System.Text.Json.JsonSerializer.Deserialize<long[]>(t.RealEstateIds));
            }
            return o;
        }));

        return response;
    }

    private readonly RealEstateValuationServiceDbContext _dbContext;

    public GetDeedOfOwnershipDocumentsHandler(RealEstateValuationServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
