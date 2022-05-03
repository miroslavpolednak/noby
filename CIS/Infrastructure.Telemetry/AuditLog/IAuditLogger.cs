namespace CIS.Infrastructure.Telemetry;

public interface IAuditLogger
{
    void Log(string message);
}
