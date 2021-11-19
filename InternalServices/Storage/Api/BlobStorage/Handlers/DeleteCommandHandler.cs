using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace CIS.InternalServices.Storage.Api.BlobStorage.Handlers;

internal class DeleteCommandHandler : AsyncRequestHandler<Dto.DeleteRequest>
{
    private readonly BlobRepository _repository;
    private readonly IBlobStorageProvider _provider;
    private readonly ILogger<DeleteCommandHandler> _logger;

    public DeleteCommandHandler(ILogger<DeleteCommandHandler> logger, IBlobStorageProvider provider, BlobRepository repository)
    {
        _repository = repository;
        _provider = provider;
        _logger = logger;
    }

    protected override async Task Handle(Dto.DeleteRequest request, CancellationToken cancellation)
    {
        var blobData = await _repository.Get(request.BlobKey);
        if (blobData is null)
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Get: Blob '{request.BlobKey}' not found", 204);
            
        _logger.LogDebug("Found blob {key} in database", request.BlobKey);

        // smazat z DB
        await _repository.Delete(request.BlobKey);

        try
        {
            await _provider.Delete(request.BlobKey, blobData.Kind, new(blobData.ApplicationKey));
        }
        catch (FileNotFoundException) // asi me nezajima, jestli se ho podarilo fyzicky smazat nebo ne.
        {
            _logger.LogWarning("Blob data {key} not found", request.BlobKey);
        }
        catch
        {
            _logger.LogWarning("Blob data {key} can not be deleted", request.BlobKey);
        }
    }
}
