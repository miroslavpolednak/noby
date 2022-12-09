using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.GetHouseholdList;

internal class GetHouseholdListHandler
    : IRequestHandler<GetHouseholdListRequest, GetHouseholdListResponse>
{
    public async Task<GetHouseholdListResponse> Handle(GetHouseholdListRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.Households
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(Database.HouseholdExpressions.HouseholdDetail())
            .ToListAsync(cancellation);

        var response = new GetHouseholdListResponse();
        response.Households.AddRange(model);
        return response;
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetHouseholdListHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}