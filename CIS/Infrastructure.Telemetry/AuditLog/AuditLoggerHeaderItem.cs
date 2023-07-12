namespace CIS.Infrastructure.Telemetry.AuditLog;

public sealed class AuditLoggerHeaderItem
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string? Id { get; set; }
    public string Type { get; set; }
}
