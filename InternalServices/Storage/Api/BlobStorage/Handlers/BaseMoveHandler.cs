namespace CIS.InternalServices.Storage.Api.BlobStorage.Handlers;

internal abstract class BaseMoveHandler<TRequest, TLogger> : AsyncRequestHandler<TRequest> where TRequest : IRequest
{
    protected readonly BlobRepository _repository;
    protected readonly IBlobStorageProvider _provider;
    protected readonly ILogger<TLogger> _logger;

    public BaseMoveHandler(ILogger<TLogger> logger, IBlobStorageProvider provider, BlobRepository repository)
    {
        _repository = repository;
        _provider = provider;
        _logger = logger;
    }

    protected async Task MoveBlobs(List<Dto.Blob> blobs)
    {
        if (blobs.Any())
        {
            blobs.ForEach(async t =>
            {
                try
                {
                    await _provider.Copy(new(t.BlobKey.GetValueOrDefault()), new(t.ApplicationKey), t.Kind, BlobKinds.Default);
                }
                catch (FileNotFoundException) // asi me nezajima, jestli se ho podarilo fyzicky smazat nebo ne.
                {
                    _logger.LogWarning("Blob data {key} not found", t.BlobKey);
                }
                catch
                {
                    _logger.LogWarning("Blob data {key} can not be moved", t.BlobKey);
                }
            });

            // vse se povedlo, smaz puvodni soubory
            blobs.ForEach(async t =>
            {
                await _provider.Delete(new(t.BlobKey.GetValueOrDefault()), t.Kind, new(t.ApplicationKey));
            });

            // presunout v DB
            await _repository.ChangeKind(blobs.Select(t => t.BlobKey.GetValueOrDefault().ToString()), BlobKinds.Temp, BlobKinds.Default);
        }
    }
}
