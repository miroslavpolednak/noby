using SharedAudit.Dto;

namespace SharedAudit;

internal interface IAuditLoggerInternal
{
    void Log(AuditEventContext context);
}
