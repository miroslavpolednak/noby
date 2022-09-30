﻿using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetObligation;

internal sealed class GetObligationHandler
    : IRequestHandler<GetObligationMediatrRequest, Obligation>
{
    public async Task<Obligation> Handle(GetObligationMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.CustomersObligations
            .Where(t => t.CustomerOnSAObligationId == request.ObligationId)
            .Select(Repositories.CustomerOnSAServiceExpressions.Obligation())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16042, $"Obligation ID {request.ObligationId} does not exist.");

        return model;
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public GetObligationHandler(Repositories.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
