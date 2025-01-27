﻿namespace SharedAudit;

public interface IAuditLogger
{
    void Log(
        AuditEventTypes eventType,
        string message,
        ICollection<AuditLoggerHeaderItem>? identities = null,
        ICollection<AuditLoggerHeaderItem>? products = null,
        AuditLoggerHeaderItem? operation = null,
        string? result = null,
        IDictionary<string, string>? bodyBefore = null,
        IDictionary<string, string>? bodyAfter = null);
}