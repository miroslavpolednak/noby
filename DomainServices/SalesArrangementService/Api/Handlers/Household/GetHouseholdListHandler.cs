using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class GetHouseholdHandler
    : IRequestHandler<Dto.GetHouseholdListMediatrRequest, Contracts.GetHouseholdListResponse>
{
    public async Task<Contracts.GetHouseholdListResponse> Handle(Dto.GetHouseholdListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdHandler), request.SalesArrangementId);
        
        // check existing SalesArrangementId
        await _saRepository.GetSalesArrangement(request.SalesArrangementId, cancellation);
        
        var model = await _repository.GetList(request.SalesArrangementId, cancellation);

        var response = new GetHouseholdListResponse();
        response.Households.AddRange(model);
        return response;
    }
    
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly Repositories.HouseholdRepository _repository;
    private readonly ILogger<GetHouseholdHandler> _logger;
    
    public GetHouseholdHandler(
        Repositories.HouseholdRepository repository,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<GetHouseholdHandler> logger)
    {
        _saRepository = saRepository;
        _repository = repository;
        _logger = logger;
    }
}