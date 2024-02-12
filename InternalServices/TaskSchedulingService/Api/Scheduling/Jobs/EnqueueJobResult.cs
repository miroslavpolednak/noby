namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal sealed class EnqueueJobResult
{
    /// <summary>
    /// Job byl uspesne spusten (TRUE)
    /// </summary>
    public bool IsSucessful { get; init; }

    /// <summary>
    /// TraceId spusteneho jobu
    /// </summary>
    public string? TraceId { get; init; }

    /// <summary>
    /// ID spusteneho jobu ve status tabulce
    /// </summary>
    public Guid? ScheduleJobStatusId { get; init; }

    /// <summary>
    /// Chybove hlaseni v pripade, ze se job nepodarilo pustit
    /// </summary>
    public string? ErrorMessage { get; init; }

    public EnqueueJobResult(in string? traceId, in Guid scheduleJobStatusId)
    {
        IsSucessful = true;
        TraceId = traceId;
        ScheduleJobStatusId = scheduleJobStatusId;
    }

    public EnqueueJobResult(in string errorMessage)
    {
        IsSucessful = false;
        ErrorMessage = errorMessage;
    }
}