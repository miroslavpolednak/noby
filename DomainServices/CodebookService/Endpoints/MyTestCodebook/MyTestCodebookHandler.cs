namespace DomainServices.CodebookService.Endpoints.MyTestCodebook;

public class MyTestCodebookHandler // nazev handleru musi byt ve formatu "{nazev_endpointu_z_Contracts.ICodebookService}Handler"
    : IRequestHandler<Contracts.Endpoints.MyTestCodebook.MyTestCodebookRequest, List<Contracts.GenericCodebookItem>> // TRequest a TResponse musi souhlasit se signaturou endpointu z Contracts.ICodebookService
{
    // mohu si vytahnout instanci custom konfigurace kdybych potreboval
    private readonly MyTestCodebookConfiguration _configuration;
    
    // mohu si pripravit instanci loggeru
    private readonly ILogger<MyTestCodebookHandler> _logger;
    
    // repository - ukazka custom DI
    private readonly MyTestCodebookRepository _repository;
    
    // distribuovana cache
    private readonly CIS.Infrastructure.Caching.IGlobalCache _cache;

    //ctr
    public MyTestCodebookHandler(
        MyTestCodebookRepository repository,
        MyTestCodebookConfiguration configuration, 
        ILogger<MyTestCodebookHandler> logger,
        CIS.Infrastructure.Caching.IGlobalCache cache)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Metodu Handle musi obsahovat kazdy endpoint - viz. MediatR a Mediator pattern
    /// </summary>
    public async Task<List<Contracts.GenericCodebookItem>> Handle(Contracts.Endpoints.MyTestCodebook.MyTestCodebookRequest request, CancellationToken cancellationToken)
    {
        List<Contracts.GenericCodebookItem>? data = null;

        // zkouska logovani
        _logger.LogInformation("Testovaci endpoint");

        // je ciselnik jiz v kesi?
        if (_cache.Exists(_cacheKey))
        {
            _logger.LogInformation("Cache key {key} found", _cacheKey);

            // vytahnout existujici data z kese
            data = await _cache.GetAllAsync<Contracts.GenericCodebookItem>(_cacheKey, CIS.Infrastructure.Caching.SerializationTypes.Protobuf);

            _logger.LogInformation("Data downloaded from cache");
        }
        else
        {
            // data z repository
            data = _repository.GetList();
            //data = await _repository.GetListFromDatabase();

            _logger.LogInformation("Creating cache key {key}", _cacheKey);

            // ulozit data do cache
            await _cache.SetAllAsync(_cacheKey, data, CIS.Infrastructure.Caching.SerializationTypes.Protobuf);

            _logger.LogInformation("New cache key added");
        }

        return data;
    }

    private const string _cacheKey = "MyTestCodebookList";
}
