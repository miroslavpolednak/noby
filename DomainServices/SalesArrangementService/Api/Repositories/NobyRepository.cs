using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal class NobyRepository
{
    #region Construction

    private readonly NobyDbContext _dbContext;

    public NobyRepository(NobyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task CreateForms(List<Entities.FormInstanceInterface> entities, CancellationToken cancellation)
    {
        _dbContext.FormInstanceInterfaces.AddRange(entities);
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task<int> GetFormsCount(CancellationToken cancellation)
    {
        var count = await _dbContext.FormInstanceInterfaces.CountAsync(cancellation);
        return count;
    }
}