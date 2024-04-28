using CIS.InternalServices.TaskSchedulingService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetJobStatuses;

internal sealed class GetJobStatusesHandler(Database.TaskSchedulingServiceDbContext _dbContext)
    : IRequestHandler<GetJobStatusesRequest, GetJobStatusesResponse>
{
    public async Task<GetJobStatusesResponse> Handle(GetJobStatusesRequest request, CancellationToken cancellation)
    {
        var query = _dbContext.ScheduleJobStatuses
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.TraceId))
        {
            query = query.Where(t => t.TraceId == request.TraceId);
        }
        if (!string.IsNullOrEmpty(request.ScheduleJobStatusId))
        {
            var sid = Guid.Parse(request.ScheduleJobStatusId);
            query = query.Where(t => t.ScheduleJobStatusId == sid);
        }
        
        var result = await query
            .OrderByDescending(t => t.StartedAt)
            .Take(request.PageSize).Skip((request.Page - 1) * request.PageSize)
            .Select(t => new GetJobStatusesResponse.Types.GetJobStatuseItem
            {
                StartedAt = t.StartedAt,
                Status = t.Status,
                StatusChangedAt = t.StatusChangedAt,
                JobId = t.ScheduleJobId.ToString(),
                JobName = t.Job.JobName,
                TraceId = t.TraceId,
                TriggerId = t.ScheduleTriggerId == null ? null : t.ScheduleTriggerId.ToString(),
                TriggerName = t.Trigger == null ? null : t.Trigger.TriggerName
            })
            .ToListAsync(cancellation);

        var response = new GetJobStatusesResponse();
        response.Items.AddRange(result);
        return response;
    }
}
