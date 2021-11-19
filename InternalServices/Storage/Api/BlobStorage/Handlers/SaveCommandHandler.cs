namespace CIS.InternalServices.Storage.Api.BlobStorage.Handlers;

internal class SaveCommandHandler : IRequestHandler<Dto.SaveRequest, BlobKey>
{
    private readonly BlobRepository _repository;
    private readonly IBlobStorageProvider _provider;
    private readonly ILogger<SaveCommandHandler> _logger;
        
    public SaveCommandHandler(ILogger<SaveCommandHandler> logger, IBlobStorageProvider provider, BlobRepository repository)
    {
        _repository = repository;
        _provider = provider;
        _logger = logger;
    }

    public async Task<BlobKey> Handle(Dto.SaveRequest request, CancellationToken cancellation)
    {
        // novy nazev souboru na FS
        var blobKey = BlobKey.CreateNew();

        _logger.LogDebug("Blob key: {key}; App key: {app};", blobKey, request.ApplicationKey);

        // ulozit soubor na FS
        await _provider.Save(request.Data , blobKey, request.Kind, request.ApplicationKey);

        _logger.LogDebug("Blob {key} created", blobKey);

        // save to db
        await _repository.Add(blobKey, request);

        return blobKey;
    }
}
