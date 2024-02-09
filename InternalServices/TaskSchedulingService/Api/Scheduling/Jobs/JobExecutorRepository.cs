using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal sealed class JobExecutorRepository
{
    private readonly TimeProvider _timeProvider;
    private readonly Core.Data.IConnectionProvider _connectionProvider;

    public JobExecutorRepository(TimeProvider timeProvider, Core.Data.IConnectionProvider connectionProvider)
    {
        _timeProvider = timeProvider;
        _connectionProvider = connectionProvider;
    }

    public void JobStarted(Guid scheduleJobStatusId, Guid jobId, Guid? triggerId = null, string? traceId = null)
    {
        var entity = new ScheduleJobStatus
        {
            ScheduleJobStatusId = scheduleJobStatusId,
            ScheduleJobId = jobId,
            Status = Statuses.InProgress.ToString(),
            ScheduleTriggerId = triggerId,
            StartedAt = _timeProvider.GetLocalNow().DateTime,
            TraceId = traceId
        };
        _connectionProvider.ExecuteDapper("INSERT INTO dbo.ScheduleJobStatus (ScheduleJobStatusId, ScheduleJobId, [Status], ScheduleTriggerId, StartedAt, TraceId, ExecutorType) VALUES (@ScheduleJobStatusId, @ScheduleJobId, @Status, @ScheduleTriggerId, @StartedAt, @TraceId, @ExecutorType)", entity);
    }

    public void UpdateJobState(Guid stateId, Statuses status)
    {
        _connectionProvider.ExecuteDapper("UPDATE dbo.ScheduleJobStatus SET [Status]=@Status, StatusChangedAt=@StatusChangedAt WHERE ScheduleJobStatusId=@ScheduleJobStatusId",
            new
            {
                ScheduleJobStatusId = stateId,
                Status = status.ToString(),
                StatusChangedAt = _timeProvider.GetLocalNow().DateTime
            });
    }

    private class ScheduleJobStatus
    {
        public Guid ScheduleJobStatusId { get; set; }
        public Guid ScheduleJobId { get; set; }
        public Guid? ScheduleTriggerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
        public string? TraceId { get; set; }
    }

    public enum Statuses
    {
        InProgress,
        Finished,
        Failed,
        Stale
    }
}
