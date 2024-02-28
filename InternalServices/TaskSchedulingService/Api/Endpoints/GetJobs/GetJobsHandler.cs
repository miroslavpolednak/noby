using CIS.Core.Data;
using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetJobs;

internal sealed class GetJobsHandler
    : IRequestHandler<GetJobsRequest, GetJobsResponse>
{
    public async Task<GetJobsResponse> Handle(GetJobsRequest request, CancellationToken cancellation)
    {
        var result = await _connectionProvider.ExecuteDapperRawSqlToListAsync<Job>(_sql, cancellation);
        
        var response = new GetJobsResponse();
        response.Jobs.AddRange(result.Select(t => new GetJobsResponse.Types.Job
        {
            JobId = t.ScheduleJobId.ToString(),
            JobName = t.JobName,
            JobType = t.JobType,
            Description = t.Description,
            IsDisabled = t.IsDisabled
        }));
        return response;
    }

    sealed class Job
    {
        public Guid ScheduleJobId { get; set; }
        public string JobName { get; set; } = null!;
        public string JobType { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDisabled { get; set; }
    }

    private const string _sql = "SELECT * FROM dbo.ScheduleJob ORDER BY JobName";

    private readonly Core.Data.IConnectionProvider _connectionProvider;

    public GetJobsHandler(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
