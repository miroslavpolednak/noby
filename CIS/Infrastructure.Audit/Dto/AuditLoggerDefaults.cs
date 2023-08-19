namespace CIS.Infrastructure.Audit.Dto;

internal sealed record AuditLoggerDefaults(
    string ServerIp,
    string CisAppKey,
    string EamApplication,
    string EamVersion,
    string EnvironmentName)
{
}
