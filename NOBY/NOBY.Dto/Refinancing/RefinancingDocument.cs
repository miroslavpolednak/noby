namespace NOBY.Dto.Refinancing;

public sealed class RefinancingDocument
{
    /// <summary>
    /// eArchiv ID dokumentu
    /// </summary>
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// EA kód dokumentu
    /// </summary>
    public int DocumentEACode { get; set; }
}
