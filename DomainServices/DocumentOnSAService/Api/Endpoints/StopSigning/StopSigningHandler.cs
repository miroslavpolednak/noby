using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StopSigning;

public class StopSigningHandler : IRequestHandler<StopSigningRequest>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public StopSigningHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FindAsync(request.DocumentOnSAId, cancellationToken);

        if (documentOnSa is null)
        {
            throw new CisNotFoundException(19003, $"DocumentOnSA {request.DocumentOnSAId} does not exist.");
        }

        documentOnSa.IsValid = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
