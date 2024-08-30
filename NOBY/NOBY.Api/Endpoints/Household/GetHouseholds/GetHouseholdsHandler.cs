using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients.v1;

namespace NOBY.Api.Endpoints.Household.GetHouseholds;

internal sealed class GetHouseholdsHandler(
    IHouseholdServiceClient _householdService,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetHouseholdsRequest, List<HouseholdInList>>
{
    public async Task<List<HouseholdInList>> Handle(GetHouseholdsRequest request, CancellationToken cancellationToken)
    {
        // vsechny households
        var households = await _householdService.GetHouseholdList(request.SalesArrangementId, cancellationToken);

        var householdTypes = await _codebookService.HouseholdTypes(cancellationToken);

        return households
            .Select(t => new HouseholdInList
            {
                HouseholdId = t.HouseholdId,
                HouseholdTypeId = t.HouseholdTypeId,
                HouseholdTypeName = householdTypes.First(x => x.Id == t.HouseholdTypeId).Name
            })
            .OrderBy(t => t.HouseholdTypeId)
            .ToList();
    }
}