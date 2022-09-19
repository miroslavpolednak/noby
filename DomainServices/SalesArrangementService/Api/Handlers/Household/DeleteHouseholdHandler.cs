using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class DeleteHouseholdHandler
    : IRequestHandler<Dto.DeleteHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        var householdInstance = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {request.HouseholdId} does not exist.");

        if (householdInstance.HouseholdTypeId == CIS.Foms.Enums.HouseholdTypes.Main)
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new CisArgumentException(16032, "Can't delete Debtor household", "HouseholdId");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

        // smazat customerOnSA
        if (householdInstance.CustomerOnSAId1.HasValue)
            await removeCustomer(householdInstance.CustomerOnSAId1.Value, cancellation);
        if (householdInstance.CustomerOnSAId2.HasValue)
            await removeCustomer(householdInstance.CustomerOnSAId2.Value, cancellation);
        // smazat domacnost
        _dbContext.Households.Remove(householdInstance);

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    async Task removeCustomer(int customerOnSAId, CancellationToken cancellation)
    {
        _dbContext.Customers.Remove(await _dbContext.Customers.FirstAsync(t => t.CustomerOnSAId == customerOnSAId, cancellation));

        var identities = await _dbContext.Database.ExecuteSqlInterpolatedAsync(@$"
DELETE FROM dbo.CustomerOnSAIdentity WHERE CustomerOnSAId={customerOnSAId};
DELETE FROM CustomerOnSAIncome WHERE CustomerOnSAId={customerOnSAId};
DELETE FROM dbo.CustomerOnSAObligation WHERE CustomerOnSAId={customerOnSAId}", cancellation);
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    
    public DeleteHouseholdHandler(Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}