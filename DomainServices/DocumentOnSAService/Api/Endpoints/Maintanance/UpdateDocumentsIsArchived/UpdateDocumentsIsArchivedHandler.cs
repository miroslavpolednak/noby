using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.Maintanance.UpdateDocumentsIsArchived;

internal sealed class UpdateDocumentsIsArchivedHandler
    : IRequestHandler<UpdateDocumentsIsArchivedRequest, Empty>
{
    public async Task<Empty> Handle(UpdateDocumentsIsArchivedRequest request, CancellationToken cancellationToken)
    {
        await _dbContext
            .DocumentOnSa
            .Where(d => request.DocumentOnSaIds.Contains(d.DocumentOnSAId!))
            .ExecuteUpdateAsync(s => s
                .SetProperty(d => d.IsArchived, d => true)
            , cancellationToken);

        return new Empty();
    }

    private readonly DocumentOnSAServiceDbContext _dbContext;

    public UpdateDocumentsIsArchivedHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
