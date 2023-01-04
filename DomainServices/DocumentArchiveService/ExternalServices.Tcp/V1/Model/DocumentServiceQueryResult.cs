namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;
public class DocumentServiceQueryResult
{
    public string CaseId { get; set; } = null!;

    public string DocumentId { get; set; } = null!;

    public int? EaCodeMainId { get; set; }

    public string Filename { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string AuthorUserLogin { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string FolderDocument { get; set; } = null!;

    public string FolderDocumentId { get; set; } = null!;

    public string DocumentDirection { get; set; } = null!;

    public string FormId { get; set; } = null!;

    public string ContractNumber { get; set; } = null!;

    public string PledgeAgreementNumber { get; set; } = null!;

    public int? Completeness { get; set; }

    public string Url { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string MinorCodes { get; set; } = null!;

}
