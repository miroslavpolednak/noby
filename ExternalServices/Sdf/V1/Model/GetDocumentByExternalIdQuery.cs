namespace ExternalServices.Sdf.V1.Model
{
    public class GetDocumentByExternalIdQuery
    {
        public string DocumentId { get; set; } = null!;
        public bool WithContent { get; set; }
    }
}
