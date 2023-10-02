using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_AuditMigrator.Database.Entities;

public enum LogType
{
    ReceivedHttpRequest = 0,
    SendingHttpResponse = 1,
    ProducingToKafka = 2,
    ProducedToKafka = 3,
    CouldNotProduceToKafka = 4,
    ReceivedReport = 5
}

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
}