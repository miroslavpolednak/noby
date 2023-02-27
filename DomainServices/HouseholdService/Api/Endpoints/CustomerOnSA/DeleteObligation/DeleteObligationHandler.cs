﻿using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteObligation;

internal sealed class DeleteObligationHandler
    : IRequestHandler<DeleteObligationRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CustomersObligations.FindAsync(new object[] { request.ObligationId }, cancellationToken);

        _dbContext.CustomersObligations.Remove(entity!);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public DeleteObligationHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
