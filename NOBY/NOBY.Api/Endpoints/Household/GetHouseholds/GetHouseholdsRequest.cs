namespace NOBY.Api.Endpoints.Household.GetHouseholds;

internal sealed record GetHouseholdsRequest(int SalesArrangementId)
    : IRequest<List<Dto.HouseholdInList>>
{
}