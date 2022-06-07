namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class UpdateHouseholdHandler
    : IRequestHandler<Dto.UpdateHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        var household = await _repository.GetHousehold(request.Request.HouseholdId, cancellation);

        //TODO nejake kontroly?
        await _repository.CheckCustomers(household.SalesArrangementId, request.Request.CustomerOnSAId1, request.Request.CustomerOnSAId2, cancellation);

        await _repository.Update(request.Request, cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }
    
    private readonly Repositories.HouseholdRepository _repository;
    private readonly ILogger<UpdateHouseholdHandler> _logger;
    
    public UpdateHouseholdHandler(
        Repositories.HouseholdRepository repository,
        ILogger<UpdateHouseholdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}