namespace CIS.InternalServices.Storage.Abstraction;

public interface IBlobTempServiceAbstraction
{
    Task<string> Save(byte[] data, string name = "", string contentType = "", string? sessionId = null, string? applicationKey = null);

    Task<Contracts.BlobGetResponse> Get(string blobKey);

    Task MoveSession(string sessionId);

    Task Move(IEnumerable<string> blobKey);
}
