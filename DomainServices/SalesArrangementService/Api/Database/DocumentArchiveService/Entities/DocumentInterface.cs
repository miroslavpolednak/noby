namespace DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService.Entities;

public class DocumentInterface
{
    public string DocumentId { get; set; } = null!;

    public byte[] DocumentData { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FileNameSuffix { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public long CaseId { get; set; }

    public DateTime CreatedOn { get; set; }

    public string AuthorUserLogin { get; set; } = null!;

    public string? ContractNumber { get; set; }

    public int Status { get; set; }

    public string? StatusErrorText { get; set; }

    public string? FormId { get; set; }

    public int EaCodeMainId { get; set; }

    public string DocumentDirection { get; set; } = null!;

    public string FolderDocument { get; set; } = null!;

    public string? FolderDocumentId { get; set; }

    public short Kdv { get; set; }

    public short SendDocumentOnly { get; set; }

    public FormInstanceInterface? DataSentence { get; set; }
}
