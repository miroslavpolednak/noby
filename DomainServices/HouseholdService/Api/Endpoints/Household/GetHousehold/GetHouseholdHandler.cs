namespace DomainServices.HouseholdService.Api.Endpoints.Household.GetHousehold;

internal sealed class GetHouseholdHandler
    : IRequestHandler<GetHouseholdMediatrRequest, Contracts.Household>
{
    public async Task<Contracts.Household> Handle(GetHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .AsNoTracking()
            .Select(Database.HouseholdExpressions.HouseholdDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {request.HouseholdId} does not exist.");

        return model;
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetHouseholdHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}