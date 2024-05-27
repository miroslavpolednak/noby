using CIS.Infrastructure.Data;
using CIS.InternalServices.NotificationService.Api.Database.Entities;

namespace CIS.InternalServices.NotificationService.Api.Database;

// dotnet tool install --global --add-source C:\path\dotnet-ef --version 7.0.2
// dotnet-ef migrations add [migration-name] --output-dir Services/Repositories/Migrations
// dotnet-ef migrations script
internal sealed class NotificationDbContext(BaseDbContextAggregate<NotificationDbContext> aggregate) : BaseDbContext<NotificationDbContext>(aggregate)
{
    public DbSet<Notification> Notifications { get; set; } = null!;

    #region legacy code
    public DbSet<Result> Results { get; set; } = null!;
    public DbSet<EmailResult> EmailResults { get; set; } = null!;
    public DbSet<SmsResult> SmsResults { get; set; } = null!;
    #endregion legacy code

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Notification>()
            .OwnsMany(notification => notification.Errors, b => b.ToJson())
            .OwnsMany(notification => notification.DocumentHashes, b => b.ToJson());

        #region legacy code
        modelBuilder.Entity<Result>()
            .UseTpcMappingStrategy();

        modelBuilder.Entity<SmsResult>()
            .ToTable(nameof(SmsResult));

        modelBuilder.Entity<EmailResult>()
            .ToTable(nameof(EmailResult));
        #endregion legacy code
    }
}