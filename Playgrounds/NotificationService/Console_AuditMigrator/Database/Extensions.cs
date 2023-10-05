using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Console_AuditMigrator.Database;

public static class Extensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Migrator")
                               ?? throw new ArgumentException("Connection string 'migrator' is missing.");
        
        return services
            .AddDbContext<LogDbContext>(options => options.UseSqlServer(connectionString));
    }
}