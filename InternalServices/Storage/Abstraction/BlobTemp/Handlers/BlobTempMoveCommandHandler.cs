using CIS.DomainServicesSecurity.Abstraction;

namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Handlers;

internal class BlobTempMoveCommandHandler : AsyncRequestHandler<Dto.BlobTempMoveRequest>
{
    private readonly ILogger<BlobTempMoveCommandHandler> _logger;
    private readonly Contracts.v1.BlobTemp.BlobTempClient _service;
    private readonly ICisUserContextHelpers _userContext;

    public BlobTempMoveCommandHandler(
        ILogger<BlobTempMoveCommandHandler> logger,
        Contracts.v1.BlobTemp.BlobTempClient service,
        ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }

    protected override async Task Handle(Dto.BlobTempMoveRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Move {blobs}", request.BlobKeys);

        var model = new Contracts.BlobMoveFromTempRequest();
        model.BlobKey.AddRange(request.BlobKeys);

        await _userContext.AddUserContext(async () => await _service.MoveAsync(model));

        _logger.LogDebug("Moved {blobs}", request.BlobKeys);
    }
}
