using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class SalesArrangementServiceRepository
{
    public async Task<int> CreateSalesArrangement(Entities.SalesArrangement entity, CancellationToken cancellation)
    {
        _dbContext.SalesArrangements.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
        return entity.SalesArrangementId;
    }

    public async Task<Contracts.SalesArrangement> GetSalesArrangement(int salesArrangementId, CancellationToken cancellation)
        => await _dbContext.SalesArrangements
            .Where(t => t.SalesArrangementId == salesArrangementId)
            .AsNoTracking()
            .Select(SalesArrangementServiceRepositoryExpressions.SalesArrangementDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16000, $"Sales arrangement ID {salesArrangementId} does not exist.");
    
    public async Task<Contracts.SalesArrangement?> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellation)
        => await _dbContext.SalesArrangements
            .Where(t => t.OfferId == offerId)
            .AsNoTracking()
            .Select(SalesArrangementServiceRepositoryExpressions.SalesArrangementDetail())
            .FirstOrDefaultAsync(cancellation);

    public async Task<Entities.SalesArrangementData?> GetSalesArrangementData(int salesArrangementId)
        => await _dbContext.SalesArrangementsData.AsNoTracking().FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementId);
    
    public async Task UpdateOfferId(int salesArrangementId, int offerId, CancellationToken cancellation)
    {
        var entity = await GetSalesArrangementEntity(salesArrangementId, cancellation);
        entity.OfferId = offerId;
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpdateSalesArrangementData(int salesArrangementId, string data)
    {
        var entity = await _dbContext.SalesArrangementsData
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementId);
        
        if (entity is null)
            _dbContext.SalesArrangementsData.Add(new Entities.SalesArrangementData
            {
                SalesArrangementId = salesArrangementId,
                Data = data
            });
        else
            entity.Data = data;

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSalesArrangementState(int salesArrangementId, int state, CancellationToken cancellation)
    {
        var entity = await GetSalesArrangementEntity(salesArrangementId, cancellation);
        entity.State = state;
        entity.StateUpdateTime = DateTime.Now;
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task<List<Contracts.SalesArrangement>> GetSalesArrangements(long caseId, int[] states, CancellationToken cancellation)
    {
        return await _dbContext.SalesArrangements
            .AsNoTracking()
            .Where(t => t.CaseId == caseId && states.Contains(t.State))
            .Select(SalesArrangementServiceRepositoryExpressions.SalesArrangementDetail())
            .ToListAsync(cancellation);
    }

    public async Task<Entities.SalesArrangement> GetSalesArrangementEntity(int salesArrangementId, CancellationToken cancellation)
        => await _dbContext.SalesArrangements.FindAsync(new object[] { salesArrangementId }, cancellation) ?? throw new CisNotFoundException(16000, $"Sales arrangement ID {salesArrangementId} does not exist.");

    private readonly SalesArrangementServiceDbContext _dbContext;

    public SalesArrangementServiceRepository(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
