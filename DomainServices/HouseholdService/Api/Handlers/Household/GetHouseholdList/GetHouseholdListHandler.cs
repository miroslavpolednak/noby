using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Handlers.Household.GetHouseholdList;

internal class GetHouseholdListHandler
    : IRequestHandler<GetHouseholdListMediatrRequest, GetHouseholdListResponse>
{
    public async Task<GetHouseholdListResponse> Handle(GetHouseholdListMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.Households
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(Repositories.HouseholdRepositoryExpressions.HouseholdDetail())
            .ToListAsync(cancellation);

        var response = new GetHouseholdListResponse();
        response.Households.AddRange(model);
        return response;
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public GetHouseholdListHandler(Repositories.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}