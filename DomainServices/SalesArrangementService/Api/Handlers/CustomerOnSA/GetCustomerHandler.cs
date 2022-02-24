namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetCustomerHandler
    : IRequestHandler<Dto.GetCustomerMediatrRequest, Contracts.CustomerOnSA>
{
    public async Task<Contracts.CustomerOnSA> Handle(Dto.GetCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomerHandler), request.CustomerOnSAId);
        
        var model = await _repository.GetCustomer(request.CustomerOnSAId, cancellation);
        
        return model;
    }
    
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly ILogger<GetCustomerHandler> _logger;
    
    public GetCustomerHandler(
        Repositories.CustomerOnSAServiceRepository repository,
        ILogger<GetCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}