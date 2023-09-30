using System.ComponentModel.DataAnnotations;

namespace Console_AuditMigrator.Database.Entities;

public class ProcessedFile
{
    [Key]
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; } = null!;
}