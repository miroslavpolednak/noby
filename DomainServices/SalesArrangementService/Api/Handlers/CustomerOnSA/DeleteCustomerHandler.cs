namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class DeleteCustomerHandler
    : IRequestHandler<Dto.DeleteCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteCustomerHandler), request.CustomerOnSAId);
        
        await _repository.DeleteCustomer(request.CustomerOnSAId, cancellation);
        
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
    
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly ILogger<DeleteCustomerHandler> _logger;
    
    public DeleteCustomerHandler(
        Repositories.CustomerOnSAServiceRepository repository,
        ILogger<DeleteCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}