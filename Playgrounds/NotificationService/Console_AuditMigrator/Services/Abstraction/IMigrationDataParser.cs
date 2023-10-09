namespace Console_AuditMigrator.Services.Abstraction;

public interface IMigrationDataParser
{
    Task ParseFromApplicationLogs();
}