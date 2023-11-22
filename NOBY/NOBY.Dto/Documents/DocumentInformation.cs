namespace NOBY.Dto.Documents;

public class DocumentInformation
{
    /// <summary>
    /// GUID dokumentu v dočasném úložišti
    /// </summary>
#pragma warning disable CA1720 // Identifier contains type name
    public Guid? Guid { get; set; }
#pragma warning restore CA1720 // Identifier contains type name

    /// <summary>
    /// Id EA kódu
    /// </summary>
    public int? EaCodeMainId { get; set; }

    /// <summary>
    /// Popis dokumentu
    /// </summary>
    public string? Description { get; set; }
}