using SharedAudit.Dto;

namespace SharedAudit;

internal sealed class AuditLoggerInternalMock : IAuditLoggerInternal
{
    public void Log(AuditEventContext context)
    {
        // In test do nothing
    }
}
