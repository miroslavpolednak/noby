using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.Household.GetHouseholds;

internal sealed class GetHouseholdsHandler
    : IRequestHandler<GetHouseholdsRequest, List<Dto.HouseholdInList>>
{
    public async Task<List<Dto.HouseholdInList>> Handle(GetHouseholdsRequest request, CancellationToken cancellationToken)
    {
        // vsechny households
        var households = await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken);

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

    private readonly ICodebookServiceClients _codebookService;
    private readonly IHouseholdServiceClient _householdService;
    
    public GetHouseholdsHandler(
        IHouseholdServiceClient householdService,
        ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
    }
}