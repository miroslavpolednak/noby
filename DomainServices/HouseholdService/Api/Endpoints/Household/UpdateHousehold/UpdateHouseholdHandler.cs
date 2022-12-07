using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : IRequestHandler<UpdateHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateHouseholdRequest request, CancellationToken cancellation)
    {
        var household = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {request.HouseholdId} does not exist.");

        //TODO nejake kontroly?
        if (request.CustomerOnSAId1.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.CustomerOnSAId1 && t.SalesArrangementId == household.SalesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #1 ID {request.CustomerOnSAId1} does not exist in this SA {household.SalesArrangementId}.");
        if (request.CustomerOnSAId2.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.CustomerOnSAId2 && t.SalesArrangementId == household.SalesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #2 ID {request.CustomerOnSAId2} does not exist in this SA {household.SalesArrangementId}.");

        household.CustomerOnSAId1 = request.CustomerOnSAId1;
        household.CustomerOnSAId2 = request.CustomerOnSAId2;
        
        household.ChildrenOverTenYearsCount = request.Data?.ChildrenOverTenYearsCount;
        household.ChildrenUpToTenYearsCount = request.Data?.ChildrenUpToTenYearsCount;
        household.PropertySettlementId = request.Data?.PropertySettlementId;
        household.AreBothPartnersDeptors = request.Data?.AreBothPartnersDeptors;
        
        household.SavingExpenseAmount = request.Expenses?.SavingExpenseAmount;
        household.InsuranceExpenseAmount = request.Expenses?.InsuranceExpenseAmount;
        household.HousingExpenseAmount = request.Expenses?.HousingExpenseAmount;
        household.OtherExpenseAmount = request.Expenses?.OtherExpenseAmount;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateHouseholdHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}