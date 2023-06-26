using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.AuditLog;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSmsAuditLogger(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ISmsAuditLogger, SmsAuditLogger>();

        return builder;
    }
}