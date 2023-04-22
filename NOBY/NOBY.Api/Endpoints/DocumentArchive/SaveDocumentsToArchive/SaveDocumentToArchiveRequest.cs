using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;

public class SaveDocumentsToArchiveRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    public List<DocumentsInformation> DocumentsInformation { get; set; } = null!;

    internal SaveDocumentsToArchiveRequest InfuseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }
}

public class DocumentsInformation
{
    public DocumentInformation DocumentInformation { get; set; } = null!;

    public string? FormId { get; set; }
}

public class DocumentInformation
{
    /// <summary>
    /// GUID dokumentu v dočasném úložišti
    /// </summary>
    public Guid? Guid { get; set; }

    /// <summary>
    /// Jméno souboru
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// Id EA kódu
    /// </summary>
    public int? EaCodeMainId { get; set; }

    /// <summary>
    /// Popis dokumentu
    /// </summary>
    public string? Description { get; set; }
}