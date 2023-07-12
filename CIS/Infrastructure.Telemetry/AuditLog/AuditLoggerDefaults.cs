namespace CIS.Infrastructure.Telemetry.AuditLog;

internal record AuditLoggerDefaults(
    string ServerIp,
    string CisAppKey, 
    string EamApplication, 
    string EnvironmentName)
{
}
