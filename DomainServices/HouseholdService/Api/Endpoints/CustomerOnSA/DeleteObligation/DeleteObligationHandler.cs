namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteObligation;

internal class DeleteObligationHandler
    : IRequestHandler<DeleteObligationMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteObligationMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16042, $"Obligation ID {request.ObligationId} does not exist.");

        _dbContext.CustomersObligations.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public DeleteObligationHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
