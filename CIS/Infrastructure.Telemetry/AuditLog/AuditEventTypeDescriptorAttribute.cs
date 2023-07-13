namespace CIS.Infrastructure.Telemetry.AuditLog;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class AuditEventTypeDescriptorAttribute
    : Attribute
{
    public string Name { get; init; } = "";
    public string Code { get; init; } = "";
    public int Version { get; init; }
    public string[]? Results { get; init; }

    public AuditEventTypeDescriptorAttribute(string name, string code, int version = 1, string[]? results = null)
    {
        Name = name;
        Code = code;
        Version = version;
        Results = results;
    }
}
