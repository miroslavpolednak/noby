using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class NobyRepository
{
    #region Construction

    private readonly NobyDbContext _dbContext;

    public NobyRepository(NobyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task CreateForm(Entities.FormInstanceInterface entity, CancellationToken cancellation)
    {
        _dbContext.FormInstanceInterfaces.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task<int> GetFormsCount(CancellationToken cancellation)
    {
        var count = await _dbContext.FormInstanceInterfaces.CountAsync(cancellation);
        return count;
    }
}