using CIS.InternalServices.Storage.Abstraction.BlobStorage.Dto;

namespace CIS.InternalServices.Storage.Abstraction.BlobStorage.Handlers;

internal class BlobDeleteCommandHandler : AsyncRequestHandler<BlobDeleteRequest>
{
    private readonly ILogger<BlobDeleteCommandHandler> _logger;
    private readonly Contracts.v1.Blob.BlobClient _service;

    public BlobDeleteCommandHandler(
        ILogger<BlobDeleteCommandHandler> logger,
        Contracts.v1.Blob.BlobClient service)
    {
        _service = service;
        _logger = logger;
    }

    protected override async Task Handle(BlobDeleteRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Delete blob {key}", request.BlobKey);

        var model = new Contracts.BlobDeleteRequest
        {
            BlobKey = request.BlobKey
        };

        await _service.DeleteAsync(model);

        _logger.LogDebug("Deleted blob {key}", request.BlobKey);
    }
}
