namespace CIS.InternalServices.Storage.Api.BlobStorage.Dto;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal record Blob()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
{
    public Guid? BlobKey { get; init; }
        
    public string ApplicationKey { get; init; }
        
    public string BlobName { get; init; }
        
    public long BlobLength { get; init; }
        
    public string BlobContentType { get; init; }

    public BlobKinds Kind { get; init; }
}
