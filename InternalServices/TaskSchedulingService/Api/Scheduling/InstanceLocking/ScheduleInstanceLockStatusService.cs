using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;

internal sealed class ScheduleInstanceLockStatusService
{
    private readonly Core.Data.IConnectionProvider _connectionProvider;
    private readonly string _instanceName;
    private readonly TimeProvider _timeProvider;

    public CurrentStateInfo CurrentState { get; private set; } = new(false, null);

    public ScheduleInstanceLockStatusService(Core.Data.IConnectionProvider connectionProvider, TimeProvider timeProvider)
    {
        _connectionProvider = connectionProvider;
        _instanceName = Environment.MachineName;
        _timeProvider = timeProvider;
    }

    public CurrentStateInfo TryAcquireLock()
    {
        var result = _connectionProvider.ExecuteDapperStoredProcedureFirstOrDefault<StatusInfo>(
            "EXEC dbo.fn_AcquireScheduleLock @instanceName=@p1, @lockTimeout=@p2",
            new { 
                p1 = _instanceName, 
                p2 = SchedulingConstants.ScheduleInstanceLockTimeout 
            })!;

        CurrentState = new(result.IsLockAcquired, result.LockOwnerInstanceName)
        {
            LastCheck = _timeProvider.GetLocalNow().DateTime
        };
        return CurrentState;
    }

    public sealed record CurrentStateInfo(bool IsLockAcquired, string? LockOwnerInstanceName)
    {
        internal DateTime? LastCheck { get; set; }
    }

    private sealed class StatusInfo
    {
        public bool IsLockAcquired { get; set; }
        public string LockOwnerInstanceName { get; set; } = null!;
    }
}
