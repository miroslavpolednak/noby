using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StopSigning;

public sealed class StopSigningHandler : IRequestHandler<StopSigningRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    public StopSigningHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Empty> Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FindAsync(request.DocumentOnSAId, cancellationToken);

        if (documentOnSa is null)
        {
            throw new CisNotFoundException(19003, $"DocumentOnSA {request.DocumentOnSAId} does not exist.");
        }

        documentOnSa.IsValid = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}
