using CIS.Infrastructure.Data;
using CIS.InternalServices.TaskSchedulingService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.TaskSchedulingService.Api.Database;

internal sealed class TaskSchedulingServiceDbContext
    : BaseDbContext<TaskSchedulingServiceDbContext>
{
    public TaskSchedulingServiceDbContext(BaseDbContextAggregate<TaskSchedulingServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<ScheduleJob> ScheduleJobs { get; set; }
    public DbSet<ScheduleTrigger> ScheduleTriggers { get; set; }
    public DbSet<ScheduleJobStatus> ScheduleJobStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScheduleTrigger>()
            .HasOne(t => t.Job)
            .WithMany()
            .HasForeignKey(t => t.ScheduleJobId)
            .IsRequired();

        modelBuilder.Entity<ScheduleJobStatus>()
            .HasOne(t => t.Job)
            .WithMany()
            .HasForeignKey(t => t.ScheduleJobId)
            .IsRequired();

        modelBuilder.Entity<ScheduleJobStatus>()
            .HasOne(t => t.Trigger)
            .WithMany()
            .HasForeignKey(t => t.ScheduleTriggerId);
    }
}
