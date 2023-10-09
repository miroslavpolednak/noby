using Console_AuditMigrator.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Console_AuditMigrator.Database;

// dotnet-ef migrations add [migration-name] --context LogDbContext --output-dir Database/Migrations
// dotnet-ef migrations script --context LogDbContext
public class LogDbContext : DbContext
{
    public DbSet<ProcessedFile> ProcessedFile { get; set; } = null!;
    public DbSet<ApplicationLog> ApplicationLog { get; set; } = null!;
    public DbSet<MigrationData> MigrationData { get; set; } = null!;

    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
    {
    }
}