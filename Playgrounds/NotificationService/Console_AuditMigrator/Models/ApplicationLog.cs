namespace Console_AuditMigrator.Models;

public class ApplicationLog
{
    public DateTime? Timestamp { get; set; }
    public string? ThreadId { get; set; }
    public string? Level { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public string? ParentId { get; set; }
    public string? CisAppKey { get; set; }
    public string? Version { get; set; }
    public string? Assembly { get; set; }
    public string? SourceContext { get; set; }
    public string? MachineName { get; set; }
    public string? ClientIp { get; set; }
    public string? CisUserId { get; set; }
    public string? CisUserIdent { get; set; }
    public string? RequestId { get; set; }
    public string? RequestPath { get; set; }
    public string? ConnectionId { get; set; }
    public string? Message { get; set; }
    public string? Exception { get; set; }
}