using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class HouseholdRepository
{
    public async Task Update(Contracts.UpdateHouseholdRequest model, CancellationToken cancellation)
    {
        var entity = await _dbContext.Households
            .Where(t => t.HouseholdId == model.HouseholdId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {model.HouseholdId} does not exist.");

        entity.CustomerOnSAId1 = model.CustomerOnSAId1;
        entity.CustomerOnSAId2 = model.CustomerOnSAId2;
        
        entity.ChildrenOverTenYearsCount = model.Data?.ChildrenOverTenYearsCount;
        entity.ChildrenUpToTenYearsCount = model.Data?.ChildrenUpToTenYearsCount;
        entity.PropertySettlementId = model.Data?.PropertySettlementId;
        entity.AreBothPartnersDeptors = model.Data?.AreBothPartnersDeptors;
        entity.AreCustomersPartners = model.Data?.AreCustomersPartners;

        entity.SavingExpenseAmount = model.Expenses?.SavingExpenseAmount;
        entity.InsuranceExpenseAmount = model.Expenses?.InsuranceExpenseAmount;
        entity.HousingExpenseAmount = model.Expenses?.HousingExpenseAmount;
        entity.OtherExpenseAmount = model.Expenses?.OtherExpenseAmount;

        await _dbContext.SaveChangesAsync(cancellation);
    }
    
    public async Task<Contracts.Household> GetHousehold(int householdId, CancellationToken cancellation)
        => await _dbContext.Households
            .Where(t => t.HouseholdId == householdId)
            .AsNoTracking()
            .Select(HouseholdRepositoryExpressions.HouseholdDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {householdId} does not exist.");
    
    public async Task<List<Contracts.Household>> GetList(int salesArrangementId, CancellationToken cancellation)
        => await _dbContext.Households
            .Where(t => t.SalesArrangementId == salesArrangementId)
            .AsNoTracking()
            .Select(HouseholdRepositoryExpressions.HouseholdDetail())
            .ToListAsync(cancellation);
    
    public async Task CheckCustomers(int salesArrangementId, int? customerId1, int? customerId2, CancellationToken cancellation)
    {
        if (customerId1.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerId1 && t.SalesArrangementId == salesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #1 ID {customerId1} does not exist in this SA {salesArrangementId}.");
        if (customerId2.HasValue
            && !(await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerId2 && t.SalesArrangementId == salesArrangementId, cancellation)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #2 ID {customerId2} does not exist in this SA {salesArrangementId}.");
    }
    
    private readonly HouseholdServiceDbContext _dbContext;

    public HouseholdRepository(HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}