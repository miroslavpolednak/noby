namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetCustomerListHandler
    : IRequestHandler<Dto.GetCustomerListMediatrRequest, Contracts.GetCustomerListResponse>
{
    public async Task<Contracts.GetCustomerListResponse> Handle(Dto.GetCustomerListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomerListHandler), request.SalesArrangementId);

        var model = new Contracts.GetCustomerListResponse();
        model.Customers.AddRange(await _repository.GetList(request.SalesArrangementId, cancellation));

        _logger.FoundItems(model.Customers.Count);
        
        return model;
    }
    
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly ILogger<GetCustomerListHandler> _logger;
    
    public GetCustomerListHandler(
        Repositories.CustomerOnSAServiceRepository repository,
        ILogger<GetCustomerListHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}