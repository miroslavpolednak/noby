using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal sealed class LinkCustomerOnSAToHouseholdHandler
    : IRequestHandler<LinkCustomerOnSAToHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(LinkCustomerOnSAToHouseholdRequest request, CancellationToken cancellationToken)
    {
        // domacnost
        var household = await _dbContext
            .Households
            .FirstOrDefaultAsync(t => t.HouseholdId == request.HouseholdId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.HouseholdNotFound, request.HouseholdId);

        // customer 1 existuje na SA
        if (request.CustomerOnSAId1.HasValue
            && !(await _dbContext.CustomerExistOnSalesArrangement(request.CustomerOnSAId1!.Value, household.SalesArrangementId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CustomerNotOnSA, household.CustomerOnSAId1);
        }

        // customer 2 existuje na SA
        if (request.CustomerOnSAId2.HasValue
            && !(await _dbContext.CustomerExistOnSalesArrangement(request.CustomerOnSAId2!.Value, household.SalesArrangementId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CustomerNotOnSA, household.CustomerOnSAId2);
        }

        household.CustomerOnSAId1 = request.CustomerOnSAId1;
        household.CustomerOnSAId2 = request.CustomerOnSAId2;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly HouseholdServiceDbContext _dbContext;
    
    public LinkCustomerOnSAToHouseholdHandler(HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
