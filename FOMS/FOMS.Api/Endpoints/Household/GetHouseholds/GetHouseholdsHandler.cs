using DomainServices.CodebookService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.GetHouseholds;

internal class GetHouseholdsHandler
    : IRequestHandler<GetHouseholdsRequest, GetHouseholdsResponse>
{
    public async Task<GetHouseholdsResponse> Handle(GetHouseholdsRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetHouseholdsHandler), request.SalesArrangementId);

        // vsechny households
        var households = ServiceCallResult.Resolve<List<contracts.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));
        _logger.FoundItems(households.Count, nameof(Household));

        var householdTypes = await _codebookService.HouseholdTypes();

        var model = new GetHouseholdsResponse
        {
            Households = households.Select(t => new Dto.HouseholdInList
            {
                HouseholdId = t.HouseholdId,
                HouseholdTypeId = t.HouseholdTypeId,
                HouseholdTypeName = householdTypes.First(x => x.Id == t.HouseholdTypeId).Name
            }).ToList()
        };
        return model;
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<GetHouseholdsHandler> _logger;
    
    public GetHouseholdsHandler(
        IHouseholdServiceAbstraction householdService,
        ICodebookServiceAbstraction codebookService,
        ILogger<GetHouseholdsHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _householdService = householdService;
    }
}