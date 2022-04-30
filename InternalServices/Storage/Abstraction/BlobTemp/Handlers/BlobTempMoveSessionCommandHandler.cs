using CIS.DomainServicesSecurity.Abstraction;

namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Handlers;

internal class BlobTempMoveSessionCommandHandler : AsyncRequestHandler<Dto.BlobTempMoveSessionRequest>
{
    private readonly ILogger<BlobTempMoveSessionCommandHandler> _logger;
    private readonly Contracts.v1.BlobTemp.BlobTempClient _service;
    private readonly ICisUserContextHelpers _userContext;

    public BlobTempMoveSessionCommandHandler(
        ILogger<BlobTempMoveSessionCommandHandler> logger,
        Contracts.v1.BlobTemp.BlobTempClient service,
        ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
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

        await _userContext.AddUserContext(async () => await _service.MoveSessionAsync(model));

        _logger.LogDebug("Moved session {sessionId}", request.SessionId);
    }
}
