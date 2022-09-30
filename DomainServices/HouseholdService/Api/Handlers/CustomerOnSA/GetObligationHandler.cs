using DomainServices.HouseholdService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA;

internal class GetObligationHandler
    : IRequestHandler<Dto.GetObligationMediatrRequest, Obligation>
{
    public async Task<Obligation> Handle(Dto.GetObligationMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .Select(Repositories.CustomerOnSAServiceRepositoryExpressions.Obligation())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16042, $"Obligation ID {request.ObligationId} does not exist.");

        return model;
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public GetObligationHandler(Repositories.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
