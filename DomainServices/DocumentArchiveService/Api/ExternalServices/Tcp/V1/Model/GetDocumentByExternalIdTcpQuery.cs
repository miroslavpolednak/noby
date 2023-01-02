namespace DomainServices.DocumentArchiveService.Api.ExternalServices.Tcp.V1.Model;

public class GetDocumentByExternalIdTcpQuery
{
    public string DocumentId { get; set; } = null!;

    public bool WithContent { get; set; }
}
