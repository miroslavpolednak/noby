using CIS.InternalServices.TaskSchedulingService.Api.Scheduling;
using CIS.InternalServices.TaskSchedulingService.Contracts;
using Google.Protobuf.WellKnownTypes;
using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.UpdateScheduler;

internal sealed class UpdateSchedulerHandler
    : IRequestHandler<UpdateSchedulerRequest, Empty>
{
    public Task<Empty> Handle(UpdateSchedulerRequest request, CancellationToken cancellationToken)
    {
        _scheduler.Stop();
        _scheduler.RemoveAllTasks();

        _triggerService.UpdateTriggersInScheduler(_scheduler, cancellationToken);

        _scheduler.Start(cancellationToken);

        return Task.FromResult(new Empty());
    }

    private readonly IScheduler _scheduler;
    private readonly TriggerService _triggerService;

    public UpdateSchedulerHandler(IScheduler scheduler, TriggerService triggerService)
    {
        _scheduler = scheduler;
        _triggerService = triggerService;
    }
}
