using CIS.Core.Exceptions;
using CIS.Infrastructure.Data;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal sealed class JobExecutorRepository
{
    private readonly TimeProvider _timeProvider;
    private readonly Core.Data.IConnectionProvider _connectionProvider;

    private const string _sqlInsert = "INSERT INTO dbo.ScheduleJobStatus (ScheduleJobStatusId, ScheduleJobId, [Status], ScheduleTriggerId, StartedAt, TraceId) VALUES (@ScheduleJobStatusId, @ScheduleJobId, @Status, @ScheduleTriggerId, @StartedAt, @TraceId)";
    private const string _sqlUpdate = "UPDATE dbo.ScheduleJobStatus SET [Status]=@Status, StatusChangedAt=@StatusChangedAt WHERE ScheduleJobStatusId=@ScheduleJobStatusId";
    private const string _sqlActiveJobs = "SELECT ScheduleJobId, JobType FROM dbo.ScheduleJob WHERE IsDisabled=0";

    public JobExecutorRepository(TimeProvider timeProvider, Core.Data.IConnectionProvider connectionProvider)
    {
        _timeProvider = timeProvider;
        _connectionProvider = connectionProvider;
    }

    public Dictionary<Guid, Type> GetActiveJobs()
    {
        return _connectionProvider
            .ExecuteDapperRawSqlToList<(Guid ScheduleJobId, string JobType)>(_sqlActiveJobs)
            .ToDictionary(k => k.ScheduleJobId, v =>
            {
                var jobType = Type.GetType(v.JobType);

                if (jobType is null || !jobType.IsAssignableTo(typeof(IJob)))
                {
                    throw new CisArgumentException(601, $"Type '{v}' can not be created or is not IJob");
                }

                return jobType;
            });
    }

    public void JobStarted(Guid scheduleJobStatusId, Guid jobId, Guid? triggerId = null, string? traceId = null)
    {
        var entity = new ScheduleJobStatus
        {
            ScheduleJobStatusId = scheduleJobStatusId,
            ScheduleJobId = jobId,
            Status = ScheduleJobStatuses.InProgress.ToString(),
            ScheduleTriggerId = triggerId,
            StartedAt = _timeProvider.GetLocalNow().DateTime,
            TraceId = traceId
        };
        _connectionProvider.ExecuteDapper(_sqlInsert, entity);
    }

    public void UpdateJobState(Guid stateId, ScheduleJobStatuses status)
    {
        _connectionProvider.ExecuteDapper(_sqlUpdate,
            new
            {
                ScheduleJobStatusId = stateId,
                Status = status.ToString(),
                StatusChangedAt = _timeProvider.GetLocalNow().DateTime
            });
    }

    private sealed class ScheduleJobStatus
    {
        public Guid ScheduleJobStatusId { get; set; }
        public Guid ScheduleJobId { get; set; }
        public Guid? ScheduleTriggerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
        public string? TraceId { get; set; }
    }
}
