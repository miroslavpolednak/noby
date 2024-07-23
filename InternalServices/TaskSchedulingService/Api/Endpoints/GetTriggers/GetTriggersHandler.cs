using CIS.InternalServices.TaskSchedulingService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetTriggers;

internal sealed class GetTriggersHandler(Database.TaskSchedulingServiceDbContext _dbContext)
    : IRequestHandler<GetTriggersRequest, GetTriggersResponse>
{
    public async Task<GetTriggersResponse> Handle(GetTriggersRequest request, CancellationToken cancellation)
    {
        var result = await _dbContext
            .ScheduleTriggers
            .AsNoTracking()
            .Include(t => t.Job)
            .ToListAsync(cancellation);
        
        var response = new GetTriggersResponse();
        response.Triggers.AddRange(result.Select(t =>
        {
            string description = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(t.Cron, new CronExpressionDescriptor.Options
            {
                DayOfWeekStartIndexZero = true,
                Use24HourTimeFormat = true
            });

            return new GetTriggersResponse.Types.Trigger
            {
                TriggerId = t.ScheduleTriggerId.ToString(),
                TriggerName = t.TriggerName,
                CronExpression = t.Cron,
                CronExpressionText = description,
                JobId = t.ScheduleJobId.ToString(),
                JobName = t.Job.JobName,
                JobType = t.Job.JobType,
                IsDisabled = t.IsDisabled
            };
        }));
        return response;
    }
}
