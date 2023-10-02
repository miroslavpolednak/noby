namespace ExternalServices.SbQueues.V1.Model;

public class Attachment
{
    public long AttachmentId { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public byte[]? Content { get; set; }
}