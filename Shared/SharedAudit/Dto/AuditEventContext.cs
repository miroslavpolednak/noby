﻿using static SharedAudit.AuditLogger;

namespace SharedAudit.Dto;

public sealed class AuditEventContext
{
    public AuditEventTypes EventType { get; init; }
    public string Message { get; init; } = null!;

    public ICollection<AuditLoggerHeaderItem>? Identities { get; init; }
    public ICollection<AuditLoggerHeaderItem>? Products { get; init; }
    public AuditLoggerHeaderItem? Operation { get; init; }
    public IDictionary<string, string>? BodyBefore { get; init; }
    public IDictionary<string, string>? BodyAfter { get; init; }
    public string? Result { get; init; }

    public string? Correlation { get; init; }
    public string ClientIp { get; init; } = null!;
    public string? UserIdent { get; init; }
    public DateTime Timestamp { get; init; }
    public long? SequenceId { get; init; }
}
