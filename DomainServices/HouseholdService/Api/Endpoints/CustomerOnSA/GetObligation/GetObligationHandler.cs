using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetObligation;

internal sealed class GetObligationHandler
    : IRequestHandler<GetObligationRequest, Obligation>
{
    public async Task<Obligation> Handle(GetObligationRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .Select(Database.CustomerOnSAServiceExpressions.Obligation())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16042, $"Obligation ID {request.ObligationId} does not exist.");

        return model;
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetObligationHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
