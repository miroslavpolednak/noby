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
            Key = fileName,
            BucketName = folderOrContainer
        };

        var response = await _client.GetObjectAsync(getRequest, cancellationToken);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);

        return memoryStream.ToArray();
    }

    public async Task SaveFile(byte[] data, string fileName, string? folderOrContainer = null, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(data);

        var putRequest = new PutObjectRequest
        {
            BucketName = folderOrContainer,
            Key = fileName,
            InputStream = memoryStream
        };

        putRequest.Headers["x-emc-retention-period"] = _retentionPeriod;

        await _client.PutObjectAsync(putRequest, cancellationToken);
    }

    private string _retentionPeriod;
    private readonly AmazonS3Client _client;

    public AmazonS3StorageClient(StorageClientConfiguration configuration)
    {
        _retentionPeriod = configuration.AmazonS3!.RetentionPeriod.ToString(CultureInfo.InvariantCulture);

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
