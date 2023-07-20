namespace NOBY.Infrastructure.Services.TempFileManager;

public struct TempFile
{
    public Guid TempFileId { get; set; }
    public string FileName { get; set; }
    public string MimeType { get; set; }
    public long? ObjectId { get; set; }
    public string? ObjectType { get; set; }
    public Guid? SessionId { get; set; }
    public byte[]? Data { get; set; }
}
