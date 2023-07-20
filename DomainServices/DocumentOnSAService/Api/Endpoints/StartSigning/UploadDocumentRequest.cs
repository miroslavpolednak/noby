namespace DomainServices.DocumentOnSAService.Api.Endpoints.StartSigning;

public class UploadDocumentRequest
{
    public long ReferenceId { get; set; }

    public string Filename { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public byte[] FileData { get; set; } = null!;
}
