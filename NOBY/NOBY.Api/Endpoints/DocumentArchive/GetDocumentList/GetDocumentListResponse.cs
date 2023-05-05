using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;

public class GetDocumentListResponse
{
    public IReadOnlyCollection<DocumentsMetadata> DocumentsMetadata { get; set; } = null!;

    public IReadOnlyCollection<CategoryEaCodeMain> CategoryEaCodeMain { get; set; } = null!;
}

public class DocumentsMetadata
{
    public string DocumentId { get; set; } = null!;

    public int? EaCodeMainId { get; set; }

    public string FileName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly CreatedOn { get; set; }

    /// <summary>
    /// Stav přenosu dokumentu do eArchivu. Enum Values: 0 - Uloženo v EArchivu, 1 - Probíhá přenos, 2 - Chyba přenosu
    /// </summary>
    /// <example>1</example>
    public UploadStatus UploadStatus { get; set; }
}

public class CategoryEaCodeMain
{
    public string Name { get; set; } = null!;

    public int DocumentCountInCategory { get; set; }

    public IReadOnlyCollection<int?> EaCodeMainIdList { get; set; } = null!;
}
