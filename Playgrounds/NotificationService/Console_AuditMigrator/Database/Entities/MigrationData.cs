using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Console_AuditMigrator.Models;

namespace Console_AuditMigrator.Database.Entities;

public class MigrationData
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(ApplicationLog))]
    public int ApplicationLogId { get; set; }

    public virtual ApplicationLog ApplicationLog { get; set; } = null!;
    
    public LogType LogType { get; set; }

    public DateTime Timestamp { get; set; }
    
    public Guid? NotificationId { get; set; }
    
    public string? RequestId { get; set; }

    public string Payload { get; set; } = null!;
}