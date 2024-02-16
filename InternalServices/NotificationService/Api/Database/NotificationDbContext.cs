using CIS.Infrastructure.Data;
using CIS.InternalServices.NotificationService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Database;

// dotnet tool install --global --add-source C:\path\dotnet-ef --version 7.0.2
// dotnet-ef migrations add [migration-name] --output-dir Services/Repositories/Migrations
// dotnet-ef migrations script
internal sealed class NotificationDbContext : BaseDbContext<NotificationDbContext>
{
    public DbSet<NotificationResult> NotificationResults { get; set; } = null!;

    public DbSet<Result> Results { get; set; } = null!;
    public DbSet<EmailResult> EmailResults { get; set; } = null!;
    public DbSet<SmsResult> SmsResults { get; set; } = null!;

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