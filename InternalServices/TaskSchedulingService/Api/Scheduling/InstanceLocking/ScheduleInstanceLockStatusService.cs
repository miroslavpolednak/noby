using CIS.Infrastructure.Data;
using Medallion.Threading.SqlServer;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;

internal sealed class ScheduleInstanceLockStatusService(
    Core.Data.IConnectionProvider _connectionProvider,
    TimeProvider _timeProvider,
    ILogger<ScheduleInstanceLockStatusService> _logger)
{
    private static readonly string _instanceName = Environment.MachineName;

    /// <summary>
    /// Status aktualni instance - zda ma aktivni lock nebo ne
    /// </summary>
    public CurrentStateInfo CurrentState { get; private set; } = new(false, null);

    public CurrentStateInfo TryAcquireLock()
    {
        try
        {
            var distributedLock = new SqlDistributedLock(nameof(ScheduleInstanceLockStatusService), _connectionProvider.ConnectionString);
            using (var handle = distributedLock.TryAcquire(TimeSpan.FromSeconds(2)))
            {
                if (handle != null)
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
                }
                else
                {
                    _logger.InstanceUnableToAcquireLock();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.InstanceLockAcquireFailed(ex);

            CurrentState = new(false, "FAILED_TO_GET_LOCK")
            {
                LastCheck = _timeProvider.GetLocalNow().DateTime
            };
        }
        
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
