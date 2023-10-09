using Console_AuditMigrator.Database;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Console_AuditMigrator.Services;

public class AuditMigrator : IAuditMigrator
{
    private readonly NotificationServiceContext _notificationServiceContext;

    public AuditMigrator(NotificationServiceContext notificationServiceContext)
    {
        _notificationServiceContext = notificationServiceContext;
    }

    public async Task Migrate()
    {
        var smsResults = await _notificationServiceContext.SmsResult
            .AsNoTracking()
            .Where(r => r.RequestTimestamp < new DateTime(2023, 10, 6))
            .OrderBy(r => r.RequestTimestamp)
            .ToListAsync();
    }
}