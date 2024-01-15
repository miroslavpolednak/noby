using Console_AuditMigrator.Database;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedAudit;

namespace Console_AuditMigrator.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDatabase(configuration)
            .AddScoped<IApplication, Application>()
            .AddTransient<ILogParser, LogParser>()
            .AddTransient<IMigrationDataParser, MigrationDataParser>()
            .AddTransient<IMigrationDataAggregator, MigrationDataAggregator>()
            .AddTransient<IAuditMigrator, AuditMigrator>();
    }
}