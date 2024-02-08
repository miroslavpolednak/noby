using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.TaskSchedulingService.Api.Database;

internal sealed class TaskSchedulingServiceDbContext
    : BaseDbContext<TaskSchedulingServiceDbContext>
{
    public TaskSchedulingServiceDbContext(BaseDbContextAggregate<TaskSchedulingServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<ScheduleTrigger> ScheduleTriggers { get; set; }

    public DbSet<ScheduleJob> ScheduleJobs { get; set; }
}
