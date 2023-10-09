using Console_AuditMigrator.Database;
using Console_AuditMigrator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Console_AuditMigrator.Services;

public class MigrationDataAggregator : IMigrationDataAggregator
{
    private readonly LogDbContext _dbContext;
    
    public MigrationDataAggregator(LogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Aggregate()
    {
        var migrationData = await _dbContext.MigrationData.ToListAsync();
        
        // todo: group by RequestId
        // todo: take type Produced, take notificationId
        // todo: for values in group fill notificationId
    }
}