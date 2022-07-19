using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class DeleteObligationHandler
    : IRequestHandler<Dto.DeleteObligationMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteObligationMediatrRequest request, CancellationToken cancellation)
    {
        //TODO kontrola zda muze smazat?

        var entity = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Obligation ID {request.ObligationId} does not exist.");
         
        _dbContext.CustomersObligations.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public DeleteObligationHandler(
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
