namespace DomainServices.HouseholdService.Api.Handlers.Household.GetHousehold;

internal sealed class GetHouseholdHandler
    : IRequestHandler<GetHouseholdMediatrRequest, Contracts.Household>
{
    public async Task<Contracts.Household> Handle(GetHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        var model = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .AsNoTracking()
            .Select(Repositories.HouseholdRepositoryExpressions.HouseholdDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16022, $"Household ID {request.HouseholdId} does not exist.");

        return model;
    }

    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public GetHouseholdHandler(Repositories.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}