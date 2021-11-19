using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Handlers;

internal class GetQueryHandler : IRequestHandler<Dto.GetRequest, Contracts.BlobGetResponse>
{
    private readonly BlobRepository _repository;
    private readonly IBlobStorageProvider _provider;
    private readonly ILogger<GetQueryHandler> _logger;

    public GetQueryHandler(ILogger<GetQueryHandler> logger, IBlobStorageProvider provider, BlobRepository repository)
    {
        _repository = repository;
        _provider = provider;
        _logger = logger;
    }

    public async Task<Contracts.BlobGetResponse> Handle(Dto.GetRequest request, CancellationToken cancellation)
    {
        var blobData = await _repository.Get(request.BlobKey);
        if (blobData is null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Get: Blob '{request.BlobKey}' not found", 204);

        _logger.LogDebug("Found blob {key}:{kind} in database", request.BlobKey, blobData.Kind);

        if (blobData.Kind != request.Kind)
        {
            _logger.LogInformation("Blob {key} is not of kind {kind}", blobData.BlobKey, request.Kind);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Get: Blob '{request.BlobKey}' not found", 204);
        }

        var blob = await _provider.Get(request.BlobKey, blobData.Kind, new(blobData.ApplicationKey));

        return new Contracts.BlobGetResponse
        {
            ContentType = blobData.BlobContentType,
            Name = blobData.BlobName,
            Data = Google.Protobuf.ByteString.CopyFrom(blob)
        };
    }
}
