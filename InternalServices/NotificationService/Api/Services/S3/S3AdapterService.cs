using Amazon.S3;
using Amazon.S3.Model;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;
using CommunityToolkit.HighPerformance.Helpers;

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

    public async Task ClearFiles(DateTime startDate, DateTime endDate, string bucketName, CancellationToken token = default)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("StartDate must be before EndDate.", nameof(startDate));
        }
        
        var objectKeyVersionsToDelete = await ListObjectKeyVersions(startDate, endDate, bucketName, token);
        var deleteRequest = new DeleteObjectsRequest { BucketName = bucketName, Objects = objectKeyVersionsToDelete };
        await _s3Client.DeleteObjectsAsync(deleteRequest, token);
    }

    private async Task<List<KeyVersion>> ListObjectKeyVersions(DateTime startDate, DateTime endDate, string bucketName, CancellationToken token = default)
    { 
        ListObjectsV2Response? response;
        var listRequest = new ListObjectsV2Request { BucketName = bucketName };
        var keyVersions = new List<KeyVersion>();
        
        do
        {
            response = await _s3Client.ListObjectsV2Async(listRequest, token);
            
            keyVersions.AddRange(response.S3Objects
                .Where(s => s.LastModified >= startDate && s.LastModified <= endDate)
                .Select(s => new KeyVersion{ Key = s.Key })
                .ToList());
            
            listRequest.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);

        return keyVersions;
    }
}