using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class GetObligationHandler
    : IRequestHandler<Dto.GetObligationMediatrRequest, Obligation>
{
    public async Task<Obligation> Handle(Dto.GetObligationMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .Select(Repositories.CustomerOnSAServiceRepositoryExpressions.Obligation())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Obligation ID {request.ObligationId} does not exist.");

        return model;
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public GetObligationHandler(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
