using DomainServices.CaseService.Clients;
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
        var flowSwitches = await _dbContext.FlowSwitches
            .Include(f => f.SalesArrangement)
            .Where(f =>
                f.FlowSwitchId == 1 &&
                f.Value && new[] { 1, 5 }.Contains(f.SalesArrangement.State) &&
                f.SalesArrangement.OfferGuaranteeDateTo < DateTime.Now)
            .ToListAsync(cancellationToken);
        
        foreach (var flowSwitch in flowSwitches)
        {
            var taskList = await _caseService.GetTaskList(flowSwitch.SalesArrangement.CaseId, cancellationToken);
            var task = taskList.FirstOrDefault(t => t is { TaskTypeId: 2, Cancelled: false });

            if (task is not null)
            {
                await _caseService.CancelTask(flowSwitch.SalesArrangement.CaseId, task.TaskIdSb, cancellationToken);
            }

            flowSwitch.Value = false;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    
    public OfferGuaranteeDateToCheckJob(
        Database.SalesArrangementServiceDbContext dbContext,
        ICaseServiceClient caseService)
    {
        _dbContext = dbContext;
        _caseService = caseService;
    }
}
