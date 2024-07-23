namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal enum ScheduleJobStatuses
{
    /// <summary>
    /// Job právě běží
    /// </summary>
    InProgress,

    /// <summary>
    /// Job byl úspěšně dokončen
    /// </summary>
    Finished,

    /// <summary>
    /// Job spadnul na chybu
    /// </summary>
    Failed,

    /// <summary>
    /// Job nebylo možné spustit, protože už běží
    /// </summary>
    FailedBecauseOfLock
}
