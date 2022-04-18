using Microsoft.Extensions.Caching.Distributed;
using CIS.Infrastructure.Caching;

namespace DomainServices.CodebookService.Endpoints.MyTestCodebook;

public class MyTestCodebookHandler // nazev handleru musi byt ve formatu "{nazev_endpointu_z_Contracts.ICodebookService}Handler"
    : IRequestHandler<Contracts.Endpoints.MyTestCodebook.MyTestCodebookRequest, List<Contracts.GenericCodebookItem>> // TRequest a TResponse musi souhlasit se signaturou endpointu z Contracts.ICodebookService
{
    /// <summary>
    /// Metodu Handle musi obsahovat kazdy endpoint - viz. MediatR a Mediator pattern
    /// </summary>
    public async Task<List<Contracts.GenericCodebookItem>> Handle(Contracts.Endpoints.MyTestCodebook.MyTestCodebookRequest request, CancellationToken cancellationToken)
    {
        List<Contracts.GenericCodebookItem>? data = null;

        // zkouska logovani
        _logger.LogInformation("Testovaci endpoint");

        // je ciselnik jiz v kesi?
        var cachedItems = await _cache.GetObjectAsync<List<Contracts.GenericCodebookItem>>(nameof(MyTestCodebookHandler), SerializationTypes.Protobuf, cancellationToken);
        if (cachedItems != null)
        {
            _logger.LogInformation("Data downloaded from cache");
            return cachedItems;
        }

        // data z repository
        data = _repository.GetList();
        //data = await _repository.GetListFromDatabase();

        // ulozit data do cache
        await _cache.SetObjectAsync(nameof(MyTestCodebookHandler), data, new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromHours(1) }, SerializationTypes.Protobuf, cancellationToken);

        return data;
    }

    // mohu si vytahnout instanci custom konfigurace kdybych potreboval
    private readonly MyTestCodebookConfiguration _configuration;
    
    // mohu si pripravit instanci loggeru
    private readonly ILogger<MyTestCodebookHandler> _logger;
    
    // repository - ukazka custom DI
    private readonly MyTestCodebookRepository _repository;
    
    // distribuovana cache
    private readonly IDistributedCache _cache;

    //ctr
    public MyTestCodebookHandler(
        MyTestCodebookRepository repository,
        MyTestCodebookConfiguration configuration, 
        ILogger<MyTestCodebookHandler> logger,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }
}
