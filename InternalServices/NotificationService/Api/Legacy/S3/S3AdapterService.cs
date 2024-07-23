using System.Globalization;
using Amazon.S3;
using Amazon.S3.Model;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Services.S3;

public class S3AdapterService : IS3AdapterService
{
    private readonly IAmazonS3 _s3Client;
    private readonly SharedComponents.Storage.Configuration.StorageClientConfiguration.StorageClientConfigurationAmazonS3 _s3Configuration;

    public S3AdapterService(IAmazonS3 s3Client, IOptions<SharedComponents.Storage.Configuration.StorageConfiguration> s3Configuration)
    {
        _s3Client = s3Client;
        _s3Configuration = s3Configuration.Value.StorageClients[nameof(IMcsStorage)].AmazonS3!;
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

        putRequest.Headers["x-emc-retention-period"] = _s3Configuration.RetentionPeriod.ToString(CultureInfo.InvariantCulture);
        
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