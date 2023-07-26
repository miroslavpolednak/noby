namespace ExternalServices.ESignatureQueues.V1.Model;

public class Attachment
{
    public long Id { get; set; }
    
    public long FileBinaryId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string ContentType { get; set; } = null!;
    
    public byte[] Content { get; set; } = null!;
}