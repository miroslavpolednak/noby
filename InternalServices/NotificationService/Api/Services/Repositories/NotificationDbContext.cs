using CIS.Infrastructure.Data;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories;

public class NotificationDbContext : BaseDbContext<NotificationDbContext>
{
    public DbSet<Result> Results { get; set; } = null!;

    public NotificationDbContext(BaseDbContextAggregate<NotificationDbContext> aggregate) : base(aggregate)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}