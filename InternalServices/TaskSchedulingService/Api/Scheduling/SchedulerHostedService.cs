using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class SchedulerHostedService(
    TriggerService _triggerService, 
    IScheduler _scheduler)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // nastaveni jobu do scheduleru (z DB)
        _triggerService.UpdateTriggersInScheduler(_scheduler);

        // spusteni scheduleru
        _scheduler.Start(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scheduler!.Stop();
        
        return Task.CompletedTask;
    }
}
