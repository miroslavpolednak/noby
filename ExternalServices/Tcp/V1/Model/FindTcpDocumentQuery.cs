namespace ExternalServicesTcp.V1.Model;

public class FindTcpDocumentQuery
{
    public long? CaseId { get; set; }

    public string? AuthorUserLogin { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public string? PledgeAgreementNumber { get; set; }

    public string? ContractNumber { get; set; }

    public int? OrderId { get; set; }

    public string? FolderDocumentId { get; set; }
}
