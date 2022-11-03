using DomainServices.CodebookService.Abstraction;
using DomainServices.HouseholdService.Clients;
using contracts = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.GetHouseholds;

internal class GetHouseholdsHandler
    : IRequestHandler<GetHouseholdsRequest, List<Dto.HouseholdInList>>
{
    public async Task<List<Dto.HouseholdInList>> Handle(GetHouseholdsRequest request, CancellationToken cancellationToken)
    {
        // vsechny households
        var households = ServiceCallResult.ResolveAndThrowIfError<List<contracts.Household>>(await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken));
        _logger.FoundItems(households.Count, nameof(Household));

        var householdTypes = await _codebookService.HouseholdTypes(cancellationToken);

        return households
            .Select(t => new Dto.HouseholdInList
            {
                HouseholdId = t.HouseholdId,
                HouseholdTypeId = t.HouseholdTypeId,
                HouseholdTypeName = householdTypes.First(x => x.Id == t.HouseholdTypeId).Name
            })
            .OrderBy(t => t.HouseholdTypeId)
            .ToList();
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ILogger<GetHouseholdsHandler> _logger;
    
    public GetHouseholdsHandler(
        IHouseholdServiceClient householdService,
        ICodebookServiceAbstraction codebookService,
        ILogger<GetHouseholdsHandler> logger)
    {
        _logger = logger;
        _codebookService = codebookService;
        _householdService = householdService;
    }
}