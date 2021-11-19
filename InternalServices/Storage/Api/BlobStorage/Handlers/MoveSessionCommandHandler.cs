using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Handlers
{
    internal class MoveSessionCommandHandler : BaseMoveHandler<Dto.MoveSessionFromTemp, MoveSessionCommandHandler>
    {
        public MoveSessionCommandHandler(ILogger<MoveSessionCommandHandler> logger, IBlobStorageProvider provider, BlobRepository repository)
            : base(logger, provider, repository)
        { }

        protected override async Task Handle(Dto.MoveSessionFromTemp request, CancellationToken cancellation)
        {
            var blobs = await _repository.GetSession(request.SessionId);
            if (!blobs.Any())
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Session '{request.SessionId}' does not contain any blobs", 209);

            _logger.LogDebug("Found {count} blobs for Session Id {sessionId}", blobs.Count, request.SessionId);

            await base.MoveBlobs(blobs);
        }
    }
}
