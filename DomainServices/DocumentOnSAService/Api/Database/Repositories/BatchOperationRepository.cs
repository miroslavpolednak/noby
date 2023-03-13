using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Database.Repositories;

public interface IBatchOperationRepository
{
    Task UpdateIsDocumentArchiveState(List<string> EArchiveIds, CancellationToken cancelationToken);
}

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
public class BatchOperationRepository : IBatchOperationRepository
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public BatchOperationRepository(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateIsDocumentArchiveState(List<string> EArchiveIds, CancellationToken cancelationToken)
    {
        var result = await _dbContext.DocumentOnSa
            .Where(d => EArchiveIds.Contains(d.EArchivId!))
            .ExecuteUpdateAsync(s => s
                .SetProperty(d => d.IsDocumentArchived, d => true)
            , cancelationToken);
    }
}
