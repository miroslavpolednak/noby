using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.GetHouseholdIdByCustomerOnSAId;

internal sealed class GetHouseholdIdByCustomerOnSAIdHandler
    : IRequestHandler<GetHouseholdIdByCustomerOnSAIdRequest, GetHouseholdIdByCustomerOnSAIdResponse>
{
    public async Task<GetHouseholdIdByCustomerOnSAIdResponse> Handle(GetHouseholdIdByCustomerOnSAIdRequest request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.Households
            .Where(t => t.CustomerOnSAId1 == request.CustomerOnSAId || t.CustomerOnSAId2 == request.CustomerOnSAId)
            .AsNoTracking()
            .Select(t => new { t.HouseholdId })
            .FirstOrDefaultAsync(cancellationToken);

        return new GetHouseholdIdByCustomerOnSAIdResponse
        {
            HouseholdId = model?.HouseholdId
        };
    }

    private readonly Database.HouseholdServiceDbContext _dbContext;

    public GetHouseholdIdByCustomerOnSAIdHandler(Database.HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
