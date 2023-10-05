namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSA;

public class GetDocumentOnSAResponse
{
    public string Filename { get; set; } = null!;
    
    public string ContentType { get; set; } = null!;

    public byte[] FileData { get; set; } = null!;
}
