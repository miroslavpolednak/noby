namespace CIS.Infrastructure.Telemetry.AuditLog;

public sealed record AuditLoggerHeaderItem(string Type, string? Id = null)
{
    public AuditLoggerHeaderItem(string type, int id)
        : this(type, id.ToString(System.Globalization.CultureInfo.InvariantCulture)) { }

    public AuditLoggerHeaderItem(string type, long id)
        : this(type, id.ToString(System.Globalization.CultureInfo.InvariantCulture)) { }
}
