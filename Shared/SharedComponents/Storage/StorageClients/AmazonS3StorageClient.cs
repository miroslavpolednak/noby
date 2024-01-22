using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SharedComponents.Storage.Configuration;
using System.Globalization;

namespace SharedComponents.Storage.StorageClients;

internal sealed class AmazonS3StorageClient<TStorage>
    : IStorageClient<TStorage>, IDisposable
{
    public async Task<byte[]> GetFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        var getRequest = new GetObjectRequest
        {
            Key = Path.Combine(folderOrContainer ?? "", fileName),
            BucketName = _bucket
        };

        var response = await _client.GetObjectAsync(getRequest, cancellationToken);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);

        return memoryStream.ToArray();
    }

    public async Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(data);
        await SaveFile(memoryStream, fileName, folderOrContainer, cancellationToken);
    }

    public async Task SaveFile(Stream data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = Path.Combine(folderOrContainer ?? "", fileName),
            InputStream = data
        };

        putRequest.Headers["x-emc-retention-period"] = _retentionPeriod;

        await _client.PutObjectAsync(putRequest, cancellationToken);
    }

    public async Task DeleteFile(string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucket,
            Key = Path.Combine(folderOrContainer ?? "", fileName)
        };
        await _client.DeleteObjectAsync(request, cancellationToken);
    }

    private readonly string _retentionPeriod;
    private readonly AmazonS3Client _client;
    private readonly string _bucket;

    public AmazonS3StorageClient(StorageClientConfiguration configuration)
    {
        _retentionPeriod = configuration.AmazonS3!.RetentionPeriod.ToString(CultureInfo.InvariantCulture);
        _bucket = configuration.AmazonS3!.Bucket;
        
        var config = new AmazonS3Config
        {
            ServiceURL = configuration.AmazonS3!.ServiceUrl,
            ForcePathStyle = true
        };

        var credentials = new BasicAWSCredentials(configuration.AmazonS3.AccessKey, configuration.AmazonS3.SecretKey);

        _client = new AmazonS3Client(credentials, config);
    }

    public void Dispose()
    {
        if (_client is not null)
        {
            _client.Dispose();
        }
    }
}
