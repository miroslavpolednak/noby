      namespace ExternalServices.Sdf.V1.Model;

public class GetDocumentByExternalIdSdfQuery
{
    public string DocumentId { get; set; } = null!;

    public bool WithContent { get; set; }

    public string UserLogin { get; set; } = null!;
}
