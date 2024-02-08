using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class SchedulerHostedService
    : IHostedService
{
    private readonly IScheduler _scheduler;
    private readonly TriggerService _triggerService;

    public SchedulerHostedService(TriggerService triggerService, IScheduler scheduler)
    {
        _triggerService = triggerService;
        _scheduler = scheduler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _triggerService.UpdateTriggersInScheduler(_scheduler, cancellationToken);

        _scheduler.Start(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scheduler!.Stop();
        
        return Task.CompletedTask;
    }
}
