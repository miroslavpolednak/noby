using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Console_AuditMigrator.Models;

namespace Console_AuditMigrator.Database.Entities;

public class ApplicationLog
{
    [Key]
    public int Id { get; set; }
    
    public LogType LogType { get; set; }
    
    [ForeignKey(nameof(ProcessedFile))]
    public int ProcessedFileId { get; set; }

    public virtual ProcessedFile ProcessedFile { get; set; } = null!;
    
    public DateTime Timestamp { get; set; }
    
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

    public string? ParsedObject { get; set; }
}