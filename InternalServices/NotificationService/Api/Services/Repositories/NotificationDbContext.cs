using CIS.Infrastructure.Data;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories;

// dotnet tool install --global --add-source C:\path\dotnet-ef --version 7.0.1
// dotnet-ef migrations add [migration-name] --output-dir Services/Repositories/Migrations
public class NotificationDbContext : BaseDbContext<NotificationDbContext>
{
    public DbSet<Result> Results { get; set; } = null!;

    public NotificationDbContext(BaseDbContextAggregate<NotificationDbContext> aggregate) : base(aggregate)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Result>()
            .UseTpcMappingStrategy();
        
        modelBuilder.Entity<SmsResult>()
            .ToTable(nameof(SmsResult));
        
        modelBuilder.Entity<EmailResult>()
            .ToTable(nameof(EmailResult));
    }
}