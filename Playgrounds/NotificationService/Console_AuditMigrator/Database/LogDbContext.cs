using Console_AuditMigrator.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Console_AuditMigrator.Database;

// dotnet-ef migrations add [migration-name] --output-dir Database/Migrations
// dotnet-ef migrations script
public class LogDbContext : DbContext
{
    public DbSet<ProcessedFile> ProcessedFiles { get; set; } = null!;
    public DbSet<ApplicationLog> ApplicationLogs { get; set; } = null!;

    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
    {
    }
}