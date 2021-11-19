namespace CIS.InternalServices.Storage.Abstraction.BlobStorage.Handlers;

internal class BlobGetQueryHandler : IRequestHandler<Dto.BlobGetRequest, Contracts.BlobGetResponse>
{
    private readonly ILogger<BlobGetQueryHandler> _logger;
    private readonly Contracts.v1.Blob.BlobClient _service;
    private readonly Security.InternalServices.ICisUserContextHelpers _userContext;

    public BlobGetQueryHandler(
        ILogger<BlobGetQueryHandler> logger,
        Contracts.v1.Blob.BlobClient service,
        Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }

    public async Task<Contracts.BlobGetResponse> Handle(Dto.BlobGetRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Get blob {key}", request.BlobKey);

        var model = new Contracts.BlobGetRequest
        {
            BlobKey = request.BlobKey
        };

        var result = await _userContext.AddUserContext(async () => await _service.GetAsync(model));

        _logger.LogDebug("Found blob {key} with name {name}", request.BlobKey, result.Name);
        return result;
    }
}
