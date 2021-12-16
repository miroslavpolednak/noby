using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class SalesArrangementServiceRepository
{
    private readonly SalesArrangementServiceDbContext _dbContext;

    public SalesArrangementServiceRepository(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task<Entities.SalesArrangement> getSalesArrangement(int salesArrangementId)
        => await _dbContext.SalesArrangements.FindAsync(salesArrangementId) ?? throw new CIS.Core.Exceptions.CisNotFoundException(13000, $"SalesArrangement #{salesArrangementId} not found");

    public async Task<int> CreateSalesArrangement(Entities.SalesArrangement entity)
    {
        _dbContext.SalesArrangements.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity.SalesArrangementId;
    }

    public async Task<Entities.SalesArrangement> GetSalesArrangement(int salesArrangementId)
        => await getSalesArrangement(salesArrangementId);

    public async Task<Entities.SalesArrangementData> GetSalesArrangementData(int salesArrangementId)
        => await _dbContext.SalesArrangementsData.AsNoTracking().FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementId) ?? throw new CIS.Core.Exceptions.CisNotFoundException(13000, $"SalesArrangement #{salesArrangementId} data not found");
    
    public async Task UpdateSalesArrangementData(int salesArrangementId, string data)
    {
        var entity = await _dbContext.SalesArrangementsData.FindAsync(salesArrangementId) ?? throw new CIS.Core.Exceptions.CisNotFoundException(13000, $"SalesArrangement data #{salesArrangementId} not found");
        entity.Data = data;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSalesArrangementState(int salesArrangementId, int state)
    {
        var entity = await getSalesArrangement(salesArrangementId);
        entity.State = state;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Entities.SalesArrangement>> GetSalesArrangementsByCaseId(long caseId)
    {
        return await _dbContext.SalesArrangements.AsNoTracking()
            .Where(t => t.CaseId == caseId)
            .ToListAsync();
    }
}
