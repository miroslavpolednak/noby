using CIS.Infrastructure.Data;
using CIS.InternalServices.NotificationService.Api.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Repositories;

public class NotificationDbContext : BaseDbContext<NotificationDbContext>
{
    public DbSet<Result> Result { get; set; } = null!;

    public NotificationDbContext(BaseDbContextAggregate<NotificationDbContext> aggregate) : base(aggregate)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}