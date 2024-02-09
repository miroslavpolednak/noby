namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal sealed class EnqueueJobResult
{
    public bool IsSucessful { get; set; }
    public string? TraceId { get; set; }
    public Guid? ScheduleJobStatusId { get; set; }
    public string? ErrorMessage { get; set; }

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