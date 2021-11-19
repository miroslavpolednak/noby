using Grpc.Core;
using CIS.Infrastructure.gRPC;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Handlers;

internal class MoveBlobsCommandHandler : BaseMoveHandler<Dto.MoveBlobsFromTemp, MoveBlobsCommandHandler>
{
    public MoveBlobsCommandHandler(ILogger<MoveBlobsCommandHandler> logger, IBlobStorageProvider provider, BlobRepository repository)
        : base(logger, provider, repository)
    { }

    protected override async Task Handle(Dto.MoveBlobsFromTemp request, CancellationToken cancellation)
    {
        var blobs = await _repository.GetList(request.BlobKey, BlobKinds.Temp);
        if (request.BlobKey.Count != blobs.Count)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Not all Blobs has been found: {request.BlobKey.Where(t => !blobs.Any(x => new BlobKey(x.BlobKey.GetValueOrDefault()) == t))}", 208);

        _logger.LogDebug("Found {count} blobs for {keys}", blobs.Count, request.BlobKey);

        await base.MoveBlobs(blobs);
    }
}
