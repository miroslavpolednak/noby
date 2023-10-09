using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Console_AuditMigrator.Database;

public class LogDbContextFactory : IDesignTimeDbContextFactory<LogDbContext>
{
    public LogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LogDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=AuditMigrator;Integrated Security=true;Trusted_Connection=True;TrustServerCertificate=True;");

        return new LogDbContext(optionsBuilder.Options);
    }
}