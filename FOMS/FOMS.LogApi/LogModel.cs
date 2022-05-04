namespace FOMS.LogApi;

public sealed class LogModel
{
    /// <summary>
    /// Level logu
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Popis udalosti
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// ID uzivatele vracene z FE API
    /// </summary>
    public int? UserId { get; set; }
}
