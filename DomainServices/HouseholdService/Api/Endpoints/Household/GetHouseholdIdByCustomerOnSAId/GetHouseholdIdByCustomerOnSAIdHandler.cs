using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.GetHouseholdIdByCustomerOnSAId;

internal sealed class GetHouseholdIdByCustomerOnSAIdHandler(Database.HouseholdServiceDbContext _dbContext)
        : IRequestHandler<GetHouseholdIdByCustomerOnSAIdRequest, GetHouseholdIdByCustomerOnSAIdResponse>
{
    public async Task<GetHouseholdIdByCustomerOnSAIdResponse> Handle(GetHouseholdIdByCustomerOnSAIdRequest request, CancellationToken cancellationToken)
    {
        // aby ten dotaz jel nejdriv podle indexu SAid
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(t => new { t.SalesArrangementId })
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        {
            return new GetHouseholdIdByCustomerOnSAIdResponse();
        }

        var model = await _dbContext.Households
            .Where(t => t.SalesArrangementId == customer.SalesArrangementId && (t.CustomerOnSAId1 == request.CustomerOnSAId || t.CustomerOnSAId2 == request.CustomerOnSAId))
            .AsNoTracking()
            .Select(t => new { t.HouseholdId })
            .FirstOrDefaultAsync(cancellationToken);

        return new GetHouseholdIdByCustomerOnSAIdResponse
        {
            HouseholdId = model?.HouseholdId
        };
    }
}
