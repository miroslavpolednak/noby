using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Console_AuditMigrator.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var migratorConnectionString = configuration.GetConnectionStringByKey("Migrator");
        var notificationServiceConnectionString = configuration.GetConnectionStringByKey("NotificationService");
        
        return services
            .AddDbContext<LogDbContext>(options => options.UseSqlServer(migratorConnectionString))
            .AddDbContext<NotificationServiceContext>(options => options.UseSqlServer(notificationServiceConnectionString));
    }

    private static string GetConnectionStringByKey(this IConfiguration configuration, string key) =>
        configuration.GetConnectionString(key)
        ?? throw new ArgumentException($"Connection string '{key}' is missing.");

}