namespace Console_AuditMigrator.Models.LogParameters;

public class ProducedParameters
{
    public string Type { get; set; } = null!;
    public Guid NotificationId { get; set; }
    public string Consumer { get; set; } = null!;
}