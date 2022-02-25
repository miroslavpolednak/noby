namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class GetHouseholdListHandler
    : IRequestHandler<Dto.GetHouseholdMediatrRequest, Contracts.Household>
{
    public async Task<Contracts.Household> Handle(Dto.GetHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdListHandler), request.HouseholdId);
        
        var model = await _repository.GetHousehold(request.HouseholdId, cancellation);
        
        return model;
    }
    
    private readonly Repositories.HouseholdRepository _repository;
    private readonly ILogger<GetHouseholdListHandler> _logger;
    
    public GetHouseholdListHandler(
        Repositories.HouseholdRepository repository,
        ILogger<GetHouseholdListHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}