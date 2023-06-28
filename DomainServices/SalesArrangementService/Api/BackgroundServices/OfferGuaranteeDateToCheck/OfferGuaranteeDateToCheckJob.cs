using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.OfferGuaranteeDateToCheck;

/// <summary>
/// Job načítá seznam všech SalesArrangementů uložených v DB, které mají FlowSwitches.IsOfferGuaranteed na true (tedy jsou zatím stále garantované) a zároveň stav SalesArrangementu je Nový nebo rozpracováno (SalesArrangement.state = 1 nebo 5) a kontroluje jejich OfferGuaranteeDateTo
/// </summary>
internal sealed class OfferGuaranteeDateToCheckJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(_sql, cancellationToken);
    }

    private const string _sql = @"
MERGE [dbo].[FlowSwitch] AS T
USING (SELECT B.SalesArrangementId, ISNULL(A.FlowSwitchId, C.[Value]) 'FlowSwitchId', CASE WHEN A.FlowSwitchId IS NULL THEN 0 ELSE A.[Value] END [Value]
	FROM dbo.SalesArrangement B
	INNER JOIN STRING_SPLIT('1,8,9,10', ',') C ON 1=1
	LEFT JOIN dbo.FlowSwitch A ON A.SalesArrangementId=B.SalesArrangementId AND A.FlowSwitchId IN (1,8,9,10) AND A.FlowSwitchId=C.[value]
	WHERE B.[State] IN (1,5) AND B.OfferGuaranteeDateTo<GETDATE()
	) AS S
ON (T.FlowSwitchId = S.FlowSwitchId AND T.SalesArrangementId = S.SalesArrangementId)  
WHEN MATCHED THEN
    UPDATE SET T.[Value]=0
WHEN NOT MATCHED THEN  
    INSERT (FlowSwitchId, SalesArrangementId, [Value]) VALUES (S.FlowSwitchId, S.SalesArrangementId, 0);";

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public OfferGuaranteeDateToCheckJob(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
