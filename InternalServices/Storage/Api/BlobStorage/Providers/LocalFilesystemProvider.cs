using CIS.Core.Types;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Providers;

internal sealed class LocalFilesystemProvider : IBlobStorageProvider
{
    private readonly BlobConfiguration _configuration;
    private readonly ILogger<LocalFilesystemProvider> _logger;

    public LocalFilesystemProvider(BlobConfiguration configuration, ILogger<LocalFilesystemProvider> logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task Save(byte[] blobData, BlobKey blobKey, BlobKinds kind, ApplicationKey applicationKey)
    {
        var helper = new PathHelper(getPath(kind), applicationKey);
        if (helper.CreateFolderIfNotExists())
            _logger.LogInformation("Created base path folder for app key '{app}'", applicationKey);

        using (var fs = new FileStream(helper.CreatePath(blobKey), FileMode.CreateNew))
        {
            await fs.WriteAsync(blobData, 0, blobData.Length);
        }
    }

    public async Task<byte[]> Get(BlobKey blobKey, BlobKinds kind, ApplicationKey applicationKey)
    {
        string path = (new PathHelper(getPath(kind), applicationKey)).CreatePath(blobKey);

        if (!File.Exists(path))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Get: Blob '{blobKey}' not found", 204);

        return await File.ReadAllBytesAsync(path);
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task Delete(BlobKey blobKey, BlobKinds kind, ApplicationKey applicationKey)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        var helper = new PathHelper(getPath(kind), applicationKey);
        string path = helper.CreatePath(blobKey);

        if (!File.Exists(path))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Delete: Blob '{blobKey}' not found", 205);

        File.Delete(path);
    }

    public async Task Copy(BlobKey blobKey, ApplicationKey applicationKey, BlobKinds kindFrom, BlobKinds kindTo)
    {
        string pathFrom = (new PathHelper(getPath(kindFrom), applicationKey)).CreatePath(blobKey);
        string pathTo = (new PathHelper(getPath(kindTo), applicationKey)).CreatePath(blobKey);

        if (!File.Exists(pathFrom))
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Copy: Blob '{blobKey}' not found", 206);

        using (FileStream source = File.OpenRead(pathFrom))
        {
            using (FileStream destination = File.Create(pathTo))
            {
                await source.CopyToAsync(destination);
            }
        }
    }

    private string getPath(BlobKinds kind) =>
        kind switch
        {
            BlobKinds.Temp => _configuration.LocalFilesystem?.BaseTempPath ?? "",
            _ => _configuration.LocalFilesystem?.BasePath ?? ""
        };

    private class PathHelper
    {
        private readonly string _applicationKey;
        private readonly string _basePath;

        public PathHelper(string basePath, ApplicationKey applicationKey)
        {
            this._basePath = basePath;
            this._applicationKey = applicationKey;
        }

        public string CreatePath(string filename)
            => Path.Combine(_basePath, _applicationKey, filename);

        public bool CreateFolderIfNotExists()
        {
            // create base path
            if (!Directory.Exists(Path.Combine(_basePath, _applicationKey)))
            {
                Directory.CreateDirectory(Path.Combine(_basePath, _applicationKey));
                return true;
            }

            return false;
        }
    }
}
