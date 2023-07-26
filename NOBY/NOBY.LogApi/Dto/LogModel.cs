namespace NOBY.LogApi;

public sealed class LogModel
{
    /// <summary>
    /// Level logu. 1=Trace,2=Debug,3=Information,4=Warning,5=Error
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Popis udalosti
    /// </summary>
    public string? Message { get; set; }
}
