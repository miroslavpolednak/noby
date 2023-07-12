namespace NOBY.LogApi;

public sealed class AuditLogModel
{
    /// <summary>
    /// Popis udalosti
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Typ auditni udalosti
    /// </summary>
    public int AuditEventTypeId { get; set; }
}
