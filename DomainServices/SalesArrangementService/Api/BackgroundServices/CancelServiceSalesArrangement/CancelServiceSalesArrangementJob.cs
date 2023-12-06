
using DomainServices.SalesArrangementService.Api.Database;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.CancelServiceSalesArrangement;

internal sealed class CancelServiceSalesArrangementJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly SalesArrangementServiceDbContext _dbContext;

    public CancelServiceSalesArrangementJob(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        
    }
}
