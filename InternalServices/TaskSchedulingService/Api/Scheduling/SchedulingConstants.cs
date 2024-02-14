namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal static class SchedulingConstants
{
    /// <summary>
    /// Vychozi timeout pro zamknuti aktivni instance. Lock je aktivni po tuto dobu v sekundach.
    /// </summary>
    public const int ScheduleInstanceLockTimeout = 85;

    /// <summary>
    /// Po tomto case (v sekundach) bude i bezich job nastaven jako STALE
    /// </summary>
    public const int DefaultStaleJobTimeout = 300;
}
