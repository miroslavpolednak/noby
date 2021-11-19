namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp;

internal class BlobTempService : IBlobTempServiceAbstraction
{
    private readonly IMediator _mediator;

    public BlobTempService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Contracts.BlobGetResponse> Get(string blobKey)
        => await _mediator.Send(new Dto.BlobTempGetRequest(blobKey));

    public async Task Move(IEnumerable<string> blobKey)
        => await _mediator.Send(new Dto.BlobTempMoveRequest(blobKey));

    public async Task MoveSession(string sessionId)
        => await _mediator.Send(new Dto.BlobTempMoveSessionRequest(sessionId));

    public async Task<string> Save(byte[] data, string name = "", string contentType = "", string? sessionId = null, string? applicationKey = null)
        => await _mediator.Send(new Dto.BlobTempSaveRequest(data, name, contentType, sessionId, applicationKey));    }
