namespace CIS.InternalServices.Storage.Abstraction;

public interface IBlobServiceAbstraction
{
    Task<string> Save(byte[] data, string name = "", string contentType = "", string? applicationKey = null);

    Task<Contracts.BlobGetResponse> Get(string blobKey);

    Task Delete(string blobKey);
}
