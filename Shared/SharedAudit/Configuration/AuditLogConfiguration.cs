namespace SharedAudit.Configuration;

public sealed class AuditLogConfiguration
{
    /// <summary>
    /// CS na databazi auditniho logu
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Kod pro ukladani v Logmanu
    /// </summary>
    public string EamApplication { get; set; } = "NOBY";

    /// <summary>
    /// Verze logovaciho schematu
    /// </summary>
    public string EamVersion { get; set; } = "3";

    /// <summary>
    /// Klic pro sifrovani zpravy
    /// </summary>
    public string HashSecretKey { get; set; } = string.Empty;
}
