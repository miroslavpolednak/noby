using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.v1.GetHousehold;

internal sealed class GetHouseholdHandler(Database.HouseholdServiceDbContext _dbContext)
    : IRequestHandler<GetHouseholdRequest, Contracts.Household>
{
    public async Task<Contracts.Household> Handle(GetHouseholdRequest request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.Households
            .Where(t => t.HouseholdId == request.HouseholdId)
            .AsNoTracking()
            .Select(Database.HouseholdExpressions.HouseholdDetail())
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound);

        return model;
    }
}