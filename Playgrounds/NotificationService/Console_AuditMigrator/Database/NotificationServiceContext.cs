using Console_AuditMigrator.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Console_AuditMigrator.Database;

// DB integration - use it for read purposes
public class NotificationServiceContext : DbContext
{
    public DbSet<SmsResult> SmsResult { get; set; } = null!;

    public NotificationServiceContext(DbContextOptions<NotificationServiceContext> options) : base(options)
    {
    }
}