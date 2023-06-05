namespace DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;

public class FindSdfDocumentsQuery
{
    public long? CaseId { get; set; }

    public string? AuthorUserLogin { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public string? PledgeAgreementNumber { get; set; }

    public string? ContractNumber { get; set; }

    public int? OrderId { get; set; }

    public string? FolderDocumentId { get; set; }

    public string? UserLogin { get; set; }

    public string? FormId { get; set; }

}
