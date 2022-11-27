namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal sealed class LinkCustomerOnSAToHouseholdHandler
    : IRequestHandler<LinkCustomerOnSAToHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(LinkCustomerOnSAToHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        // domacnost
        var householdEntity = await _dbContext.Households
            .FirstOrDefaultAsync(t => t.HouseholdId == request.Request.HouseholdId, cancellation)
            ?? throw new CisNotFoundException(16022, $"Household ID {request.Request.HouseholdId} does not exist.");

        // overeni existence customeru
        if (request.Request.CustomerOnSAId1.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId1 && t.SalesArrangementId == householdEntity.SalesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #1 ID {request.Request.CustomerOnSAId1} does not exist in this SA {householdEntity.SalesArrangementId}.");
        if (request.Request.CustomerOnSAId2.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId2 && t.SalesArrangementId == householdEntity.SalesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #2 ID {request.Request.CustomerOnSAId2} does not exist in this SA {householdEntity.SalesArrangementId}.");

        householdEntity.CustomerOnSAId1 = request.Request.CustomerOnSAId1;
        householdEntity.CustomerOnSAId2 = request.Request.CustomerOnSAId2;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;
    
    public LinkCustomerOnSAToHouseholdHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
