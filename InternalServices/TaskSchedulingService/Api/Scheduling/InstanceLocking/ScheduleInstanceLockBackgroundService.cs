namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;

internal sealed class ScheduleInstanceLockBackgroundService
    : BackgroundService
{
    private readonly ScheduleInstanceLockStatusService _lockService;
    private readonly ILogger<ScheduleInstanceLockBackgroundService> _logger;

    public ScheduleInstanceLockBackgroundService(ScheduleInstanceLockStatusService lockService, ILogger<ScheduleInstanceLockBackgroundService> logger)
    {
        _lockService = lockService;
        _logger = logger;
    }

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
