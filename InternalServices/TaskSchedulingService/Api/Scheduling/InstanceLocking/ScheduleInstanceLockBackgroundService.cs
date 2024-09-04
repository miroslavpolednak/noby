namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;

internal sealed class ScheduleInstanceLockBackgroundService(
    ScheduleInstanceLockStatusService _lockService, 
    ILogger<ScheduleInstanceLockBackgroundService> _logger)
        : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = _lockService.TryAcquireLock();

            _logger.TryToAcquireScheduleLock(result.IsLockAcquired, result.LockOwnerInstanceName);

            await Task.Delay((SchedulingConstants.ScheduleInstanceLockTimeout - 15) * 1000, stoppingToken);
        }
    }
}
