using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;

internal sealed class ScheduleInstanceLockStatusService
{
    private readonly Core.Data.IConnectionProvider _connectionProvider;
    private readonly string _instanceName;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Status aktualni instance - zda ma aktivni lock nebo ne
    /// </summary>
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
            "dbo.fn_AcquireScheduleLock",
            new 
            { 
                instanceName = _instanceName, 
                lockTimeout = SchedulingConstants.ScheduleInstanceLockTimeout
            })!;

        CurrentState = new(result.IsLockAcquired, result.LockOwnerInstanceName)
        {
            LastCheck = _timeProvider.GetLocalNow().DateTime
        };
        return CurrentState;
    }

    public sealed record CurrentStateInfo(bool IsLockAcquired, string? LockOwnerInstanceName)
    {
        /// <summary>
        /// Cas posledniho porizeni zamku aktualni instance
        /// </summary>
        internal DateTime? LastCheck { get; init; }
    }

    private sealed class StatusInfo
    {
        /// <summary>
        /// Aktualni instance je ta aktivni
        /// </summary>
        public bool IsLockAcquired { get; set; }

        /// <summary>
        /// Nazev aktivni instance pokud se nejedna o tu aktualni
        /// </summary>
        public string LockOwnerInstanceName { get; set; } = null!;
    }
}
