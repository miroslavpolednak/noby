namespace DomainServices.HouseholdService.Api.Handlers.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : IRequestHandler<UpdateHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        var household = await _dbContext.Households
            .Where(t => t.HouseholdId == request.Request.HouseholdId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {request.Request.HouseholdId} does not exist.");

        //TODO nejake kontroly?
        if (request.Request.CustomerOnSAId1.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId1 && t.SalesArrangementId == household.SalesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #1 ID {request.Request.CustomerOnSAId1} does not exist in this SA {household.SalesArrangementId}.");
        if (request.Request.CustomerOnSAId2.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId2 && t.SalesArrangementId == household.SalesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #2 ID {request.Request.CustomerOnSAId2} does not exist in this SA {household.SalesArrangementId}.");

        household.CustomerOnSAId1 = request.Request.CustomerOnSAId1;
        household.CustomerOnSAId2 = request.Request.CustomerOnSAId2;
        
        household.ChildrenOverTenYearsCount = request.Request.Data?.ChildrenOverTenYearsCount;
        household.ChildrenUpToTenYearsCount = request.Request.Data?.ChildrenUpToTenYearsCount;
        household.PropertySettlementId = request.Request.Data?.PropertySettlementId;
        household.AreBothPartnersDeptors = request.Request.Data?.AreBothPartnersDeptors;
        household.AreCustomersPartners = request.Request.Data?.AreCustomersPartners;
        
        household.SavingExpenseAmount = request.Request.Expenses?.SavingExpenseAmount;
        household.InsuranceExpenseAmount = request.Request.Expenses?.InsuranceExpenseAmount;
        household.HousingExpenseAmount = request.Request.Expenses?.HousingExpenseAmount;
        household.OtherExpenseAmount = request.Request.Expenses?.OtherExpenseAmount;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public UpdateHouseholdHandler(Repositories.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}