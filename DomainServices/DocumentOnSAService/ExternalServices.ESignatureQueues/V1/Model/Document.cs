namespace ExternalServices.ESignatureQueues.V1.Model;

public class Document
{
    public long Id { get; set; }

    public string ExternalId { get; set; } = null!;
    
    public long FileBinaryOriginalId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string ContentType { get; set; } = null!;
    
    public byte[]? Content { get; set; }
}