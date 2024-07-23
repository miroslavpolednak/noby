namespace NOBY.Dto;

public sealed class FeBannerItem
{
    /// <summary>
    /// Nadpis upozornění
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Obsah upozornění
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Úroveň závažnosti upozornění
    /// </summary>
    public FeBannersSeverity Severity { get; set; }

    public enum FeBannersSeverity
    {
        Info = 1,
        Warning = 2,
        Error = 3
    }
}
