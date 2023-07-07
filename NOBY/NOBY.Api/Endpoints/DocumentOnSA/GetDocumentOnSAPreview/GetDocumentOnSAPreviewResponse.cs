namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAPreview;

public class GetDocumentOnSAPreviewResponse
{
    public string Filename { get; set; } = null!;
    
    public string ContentType { get; set; } = null!;

    public byte[] FileData { get; set; } = null!;
}
