using CIS.InternalServices.TaskSchedulingService.Api.Scheduling;
using CIS.InternalServices.TaskSchedulingService.Contracts;
using Google.Protobuf.WellKnownTypes;
using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.UpdateScheduler;

internal sealed class UpdateSchedulerHandler(
    IScheduler _scheduler, 
    TriggerService _triggerService)
    : IRequestHandler<UpdateSchedulerRequest, Empty>
{
    public Task<Empty> Handle(UpdateSchedulerRequest request, CancellationToken cancellationToken)
    {
        _scheduler.Stop();
        _scheduler.RemoveAllTasks();

        _triggerService.UpdateTriggersInScheduler(_scheduler);

        _scheduler.Start(cancellationToken);

        return Task.FromResult(new Empty());
    }
}
