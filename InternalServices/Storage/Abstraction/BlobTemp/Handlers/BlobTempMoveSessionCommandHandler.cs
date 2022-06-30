namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Handlers;

internal class BlobTempMoveSessionCommandHandler : AsyncRequestHandler<Dto.BlobTempMoveSessionRequest>
{
    private readonly ILogger<BlobTempMoveSessionCommandHandler> _logger;
    private readonly Contracts.v1.BlobTemp.BlobTempClient _service;

    public BlobTempMoveSessionCommandHandler(
        ILogger<BlobTempMoveSessionCommandHandler> logger,
        Contracts.v1.BlobTemp.BlobTempClient service)
    {
        _service = service;
        _logger = logger;
    }

    protected override async Task Handle(Dto.BlobTempMoveSessionRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Move session {sessionId}", request.SessionId);

        var model = new Contracts.BlobMoveSessionFromTempRequest
        {
            SessionId = request.SessionId
        };

        await _service.MoveSessionAsync(model);

        _logger.LogDebug("Moved session {sessionId}", request.SessionId);
    }
}
