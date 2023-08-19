namespace CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;

public interface IS3AdapterService
{
    public Task<string> UploadFile(byte[] content, string bucketName, CancellationToken token = default);

    public Task<byte[]> GetFile(string key, string bucketName, CancellationToken token = default);

    public Task ClearFiles(DateTime startDate, DateTime endDate, string bucketName, CancellationToken token = default);
}