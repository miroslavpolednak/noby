using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class JobExecutorRepository
{
    private readonly TimeProvider _timeProvider;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;

    public JobExecutorRepository(TimeProvider timeProvider, Core.Data.IConnectionProvider connectionProvider)
    {
        _timeProvider = timeProvider;
        _connectionProvider = connectionProvider;
    }

    public void JobStarted(Guid scheduleJobStatusId, Guid jobId, Guid triggerId, string? traceId = null, string? executorType = null)
    {
        var entity = new Database.Entities.ScheduleJobStatus
        {
            ScheduleJobStatusId = scheduleJobStatusId,
            ScheduleJobId = jobId,
            Status = Statuses.InProgress.ToString(),
            ScheduleTriggerId = triggerId,
            StartedAt = _timeProvider.GetLocalNow().DateTime,
            TraceId = traceId,
            ExecutorType = executorType ?? _defaultExecutorType
        };
        _connectionProvider.ExecuteDapper("INSERT INTO dbo.ScheduleJobStatus (ScheduleJobStatusId, ScheduleJobId, [Status], ScheduleTriggerId, StartedAt, TraceId, ExecutorType) VALUES (@ScheduleJobStatusId, @ScheduleJobId, @Status, @ScheduleTriggerId, @StartedAt, @TraceId, @ExecutorType)", entity);
    }

    public void UpdateJobState(Guid stateId, Statuses status)
    {
        _connectionProvider.ExecuteDapper("UPDATE dbo.ScheduleJobStatus [Status]=@State, StatusChangedAt=@StatusChangedAt WHERE ScheduleJobStatusId=@ScheduleJobStatusId", 
            new { 
                ScheduleJobStatusId = stateId, 
                Status = status.ToString(), 
                StatusChangedAt = _timeProvider.GetLocalNow().DateTime 
            });
    }

    private const string _defaultExecutorType = "Scheduler";

    public enum Statuses
    {
        InProgress,
        Finished,
        Failed,
        Stale
    }
}
