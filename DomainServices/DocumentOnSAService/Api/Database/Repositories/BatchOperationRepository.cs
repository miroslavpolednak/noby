using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Database.Repositories;

public interface IBatchOperationRepository
{
    Task UpdateIsDocumentArchiveState(IEnumerable<int> documentOnSaIds, CancellationToken cancelationToken);
}

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
public class BatchOperationRepository : IBatchOperationRepository
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public BatchOperationRepository(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateIsDocumentArchiveState(IEnumerable<int> documentOnSaIds, CancellationToken cancelationToken)
    {
        var result = await _dbContext.DocumentOnSa
            .Where(d => documentOnSaIds.Contains(d.DocumentOnSAId!))
            .ExecuteUpdateAsync(s => s
                .SetProperty(d => d.IsArchived, d => true)
            , cancelationToken);
    }
}
