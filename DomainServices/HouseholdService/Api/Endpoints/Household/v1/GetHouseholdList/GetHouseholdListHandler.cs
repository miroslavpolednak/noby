using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.v1.GetHouseholdList;

internal sealed class GetHouseholdListHandler(Database.HouseholdServiceDbContext _dbContext)
    : IRequestHandler<GetHouseholdListRequest, GetHouseholdListResponse>
{
    public async Task<GetHouseholdListResponse> Handle(GetHouseholdListRequest request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.Households
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(Database.HouseholdExpressions.HouseholdDetail())
            .ToListAsync(cancellationToken);

        return new()
        {
            Households = { model }
        };
    }
}