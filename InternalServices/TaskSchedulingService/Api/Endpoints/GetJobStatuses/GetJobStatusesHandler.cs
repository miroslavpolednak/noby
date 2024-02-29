using CIS.Core.Data;
using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetJobStatuses;

internal sealed class GetJobStatusesHandler
    : IRequestHandler<GetJobStatusesRequest, GetJobStatusesResponse>
{
    public async Task<GetJobStatusesResponse> Handle(GetJobStatusesRequest request, CancellationToken cancellation)
    {
        string sql = $@"SELECT B.JobName, C.TriggerName, A.ScheduleJobId, A.ScheduleTriggerId, A.[Status], A.StartedAt, A.StatusChangedAt, A.TraceId
FROM dbo.ScheduleJobStatus A
INNER JOIN dbo.ScheduleJob B ON A.ScheduleJobId=B.ScheduleJobId
INNER JOIN dbo.ScheduleTrigger C ON A.ScheduleTriggerId=C.ScheduleTriggerId
WHERE ISNULL(@TraceId, '') = '' OR @TraceId = A.TraceId
ORDER BY StartedAt DESC
OFFSET {((request.Page - 1) * request.PageSize)} ROWS FETCH NEXT {request.PageSize} ROWS ONLY";

        var result = await _connectionProvider.ExecuteDapperRawSqlToListAsync<StatusItem>(sql, new { request.TraceId }, cancellation);

        var response = new GetJobStatusesResponse();
        response.Items.AddRange(result.Select(t => new GetJobStatusesResponse.Types.GetJobStatuseItem
        {
            StartedAt = t.StartedAt,
            Status = t.Status,
            StatusChangedAt = t.StatusChangedAt,
            JobId = t.ScheduleJobId.ToString(),
            JobName = t.JobName,
            TraceId = t.TraceId,
            TriggerId = t.ScheduleTriggerId.ToString(),
            TriggerName = t.TriggerName
        }));
        return response;
    }

    sealed class StatusItem
    {
        public Guid ScheduleJobId { get; set; }
        public string JobName { get; set; } = null!;
        public Guid ScheduleTriggerId { get; set; }
        public string TriggerName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
        public string TraceId { get; set; } = null!;
    }

    private readonly Core.Data.IConnectionProvider _connectionProvider;

    public GetJobStatusesHandler(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
