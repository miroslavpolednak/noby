using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class HouseholdRepository
{
    public async Task<int> CreateHousehold(Entities.Household entity, CancellationToken cancellation)
    {
        _dbContext.Households.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
        return entity.HouseholdId;
    }

    public async Task Update(Contracts.UpdateHouseholdRequest model, CancellationToken cancellation)
    {
        var entity = await _dbContext.Households
            .Where(t => t.HouseholdId == model.HouseholdId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {model.HouseholdId} does not exist.");
        
        entity.CustomerOnSAId1 = model.CustomerOnSAId1;
        entity.CustomerOnSAId2 = model.CustomerOnSAId2;
        entity.ChildrenOverTenYearsCount = model.Data.ChildrenOverTenYearsCount;
        entity.ChildrenUpToTenYearsCount = model.Data.ChildrenUpToTenYearsCount;
        entity.PropertySettlementId = model.Data.PropertySettlementId;
        entity.SavingExpenseAmount = model.Expenses.SavingExpenseAmount;
        entity.InsuranceExpenseAmount = model.Expenses.InsuranceExpenseAmount;
        entity.HousingExpenseAmount = model.Expenses.HousingExpenseAmount;
        entity.OtherExpenseAmount = model.Expenses.OtherExpenseAmount;

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
    
    public async Task DeleteHousehold(int householdId, CancellationToken cancellation)
    {
        var entity = await _dbContext.Households
            .Where(t => t.HouseholdId == householdId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {householdId} does not exist.");

        _dbContext.Households.Remove(entity);
        
        await _dbContext.SaveChangesAsync(cancellation);
    }
    
    private readonly SalesArrangementServiceDbContext _dbContext;

    public HouseholdRepository(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}