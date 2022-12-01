namespace ExternalServicesTcp.Model;

public class GetDocumentByExternalIdTcpQuery
{
    public string DocumentId { get; set; } = null!;

    public bool WithContent { get; set; }
}
