using CIS.InternalServices.TaskSchedulingService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetJobs;

internal sealed class GetJobsHandler
    : IRequestHandler<GetJobsRequest, GetJobsResponse>
{
    public async Task<GetJobsResponse> Handle(GetJobsRequest request, CancellationToken cancellationToken)
    {
        var result = await _dbContext
            .ScheduleJobs
            .AsNoTracking()
            .OrderBy(t => t.JobName)
            .ToListAsync(cancellationToken);
        
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

    private readonly Database.TaskSchedulingServiceDbContext _dbContext;

    public GetJobsHandler(Database.TaskSchedulingServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
