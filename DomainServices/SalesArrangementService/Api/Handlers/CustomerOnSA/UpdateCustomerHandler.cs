namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class UpdateCustomerHandler
    : IRequestHandler<Dto.UpdateCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStarted(nameof(UpdateCustomerHandler));

        //TODO nejake kontroly?
        
        await _repository.UpdateCustomer(request.Request, cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
    
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly ILogger<UpdateCustomerHandler> _logger;
    
    public UpdateCustomerHandler(
        Repositories.CustomerOnSAServiceRepository repository,
        ILogger<UpdateCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}