using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Maintanance.GetCheckDocumentsArchivedIds;

internal sealed class GetCheckDocumentsArchivedIdsHandler
    : IRequestHandler<GetCheckDocumentsArchivedIdsRequest, GetCheckDocumentsArchivedIdsResponse>
{
    public async Task<GetCheckDocumentsArchivedIdsResponse> Handle(GetCheckDocumentsArchivedIdsRequest request, CancellationToken cancellationToken)
    {
        var unArchivedEArchiveLinkeds = await _dbContext
            .EArchivIdsLinked
            .AsNoTracking()
            .Where(d => d.DocumentOnSa.IsArchived == false)
            .Select(t => new GetCheckDocumentsArchivedIdsResponse.Types.GetCheckDocumentsArchivedIdsItem
            {
                DocumentOnSAId = t.DocumentOnSAId,
                EArchivId = t.EArchivId
            })
            .Take(request.MaxBatchSize)
            .ToListAsync(cancellationToken);

        GetCheckDocumentsArchivedIdsResponse response = new();
        response.Items.AddRange(unArchivedEArchiveLinkeds);
        return response;
    }

    private readonly DocumentOnSAServiceDbContext _dbContext;

    public GetCheckDocumentsArchivedIdsHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
