namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class DeleteHouseholdHandler
    : IRequestHandler<Dto.DeleteHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(DeleteCustomerHandler), request.HouseholdId);
        
        await _repository.DeleteHousehold(request.HouseholdId, cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.HouseholdRepository _repository;
    private readonly ILogger<DeleteHouseholdHandler> _logger;
    
    public DeleteHouseholdHandler(
        Repositories.HouseholdRepository repository,
        ILogger<DeleteHouseholdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}