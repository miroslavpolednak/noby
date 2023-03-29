namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentOnSAData;

public class GetDocumentOnSADataResponse
{
    public string Filename { get; set; } = null!;
    
    public string ContentType { get; set; } = null!;

    public byte[] FileData { get; set; } = null!;
}
