using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.Household.SaveHouseholds;

internal class SaveHouseholdsHandler
: IRequestHandler<SaveHouseholdsRequest, List<int>>
{
    public async Task<List<int>> Handle(SaveHouseholdsRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(SaveHouseholdsHandler), request.SalesArrangementId);

        return null;
    }
    
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<SaveHouseholdsHandler> _logger;
    
    public SaveHouseholdsHandler(
        IHouseholdServiceAbstraction householdService,
        ILogger<SaveHouseholdsHandler> logger)
    {
        _logger = logger;
        _householdService = householdService;
    }
}