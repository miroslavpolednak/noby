namespace CIS.InternalServices.Storage.Abstraction.BlobStorage.Handlers;

internal class BlobSaveCommandHandler : IRequestHandler<Dto.BlobSaveRequest, string>
{
    private readonly ILogger<BlobSaveCommandHandler> _logger;
    private readonly Core.Configuration.ICisEnvironmentConfiguration _configuration;
    private readonly Contracts.v1.Blob.BlobClient _service;

    public BlobSaveCommandHandler(
        ILogger<BlobSaveCommandHandler> logger,
        Contracts.v1.Blob.BlobClient service, 
        Core.Configuration.ICisEnvironmentConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> Handle(Dto.BlobSaveRequest request, CancellationToken cancellation)
    {
        string? key = request.ApplicationKey ?? _configuration.DefaultApplicationKey;
        _logger.LogDebug("Saving blob {name} in app {key}", request.Name, key);

        var model = new Contracts.BlobSaveRequest
        {
            ApplicationKey = key,
            BlobData = new Contracts.BlobFileStructure
            {
                ContentType = request.ContentType,
                Name = request.Name,
                Data = Google.Protobuf.ByteString.CopyFrom(request.Data)
            }
        };

        var result = await _service.SaveAsync(model);

        _logger.LogDebug("Saved with key {key}", result.BlobKey);
        return result.BlobKey;
    }
}
