namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Handlers;

internal class BlobTempGetQueryHandler : IRequestHandler<Dto.BlobTempGetRequest, Contracts.BlobGetResponse>
{
    private readonly ILogger<BlobTempGetQueryHandler> _logger;
    private readonly Contracts.v1.BlobTemp.BlobTempClient _service;
        
    public BlobTempGetQueryHandler(
        ILogger<BlobTempGetQueryHandler> logger,
        Contracts.v1.BlobTemp.BlobTempClient service)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<Contracts.BlobGetResponse> Handle(Dto.BlobTempGetRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Get blob {key}", request.BlobKey);

        var model = new Contracts.BlobGetRequest
        {
            BlobKey = request.BlobKey
        };

        var result = await _service.GetAsync(model);

        _logger.LogDebug("Found blob {key} with name {name}", request.BlobKey, result.Name);
        return result;
    }
}
