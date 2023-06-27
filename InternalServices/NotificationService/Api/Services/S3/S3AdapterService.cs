using Amazon.S3;
using Amazon.S3.Model;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.S3;

public class S3AdapterService : IS3AdapterService
{
    private readonly IAmazonS3 _s3Client;

    public S3AdapterService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task<string> UploadFile(byte [] content, string bucketName, CancellationToken token = default)
    {
        var key = Guid.NewGuid().ToString();
        using var memoryStream = new MemoryStream(content);

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = memoryStream
        };
        
        var putResponse = await _s3Client.PutObjectAsync(putRequest, token);
        
        return key;
    }

    public async Task<byte[]> GetFile(string key, string bucketName, CancellationToken token = default)
    {
        var getRequest = new GetObjectRequest
        {
            Key = key,
            BucketName = bucketName
        };
        
        var response = await _s3Client.GetObjectAsync(getRequest, token);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, token);

        return memoryStream.ToArray();
    }
}