using Console_AuditMigrator.Services.Abstraction;

namespace Console_AuditMigrator.Services;

/// <summary>
/// Read the README.md file
/// Read the README.md file
/// Read the README.md file
/// </summary>
public class Application : IApplication
{
    private readonly ILogParser _logParser;
    private readonly IMigrationDataParser _migrationDataParser;
    private readonly IMigrationDataAggregator _migrationDataAggregator;
    private readonly IAuditMigrator _auditMigrator;
    
    public Application(
        ILogParser logParser,
        IMigrationDataParser migrationDataParser,
        IMigrationDataAggregator migrationDataAggregator,
        IAuditMigrator auditMigrator)
    {
        _logParser = logParser;
        _migrationDataParser = migrationDataParser;
        _migrationDataAggregator = migrationDataAggregator;
        _auditMigrator = auditMigrator;
    }
    
    public async Task Run()
    {
        // Read the README.md file
        // Read the README.md file
        // Read the README.md file
        
        // step 1 - parse application logs from files
        //await _logParser.ParseLogFiles();

        // step 2 - parse migration data from application logs
        //await _migrationDataParser.ParseFromApplicationLogs();
        
        // step 3 - aggregate migration data and fill notificationId
        //await _migrationDataAggregator.Aggregate();
        
        // step 4 - migrate data
        //await _auditMigrator.Migrate();
    }
}