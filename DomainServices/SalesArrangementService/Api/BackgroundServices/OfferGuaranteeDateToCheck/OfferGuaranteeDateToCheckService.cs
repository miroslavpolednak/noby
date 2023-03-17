using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.OfferGuaranteeDateToCheck;

internal sealed class OfferGuaranteeDateToCheckService
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(_sql, cancellationToken);
    }

    private const string _sql = @"
UPDATE dbo.FlowSwitch SET [Value]=0
FROM dbo.FlowSwitch X1
INNER JOIN (
	SELECT A.SalesArrangementId, A.FlowSwitchId
	FROM dbo.FlowSwitch A
	INNER JOIN dbo.SalesArrangement B ON A.SalesArrangementId=B.SalesArrangementId
	WHERE A.FlowSwitchId=1 AND A.[Value]=1 AND B.[State] IN (1,5) AND B.OfferGuaranteeDateTo<GETDATE()
) X2 ON X1.FlowSwitchId=X2.FlowSwitchId AND X1.SalesArrangementId=X2.SalesArrangementId";

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public OfferGuaranteeDateToCheckService(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
