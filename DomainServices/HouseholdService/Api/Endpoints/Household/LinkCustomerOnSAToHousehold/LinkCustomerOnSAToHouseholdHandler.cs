using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal sealed class LinkCustomerOnSAToHouseholdHandler
    : IRequestHandler<LinkCustomerOnSAToHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(LinkCustomerOnSAToHouseholdRequest request, CancellationToken cancellationToken)
    {
        // domacnost
        var householdEntity = await _dbContext.GetHousehold(request.HouseholdId, cancellationToken);

        // overeni existence customeru
        if (request.CustomerOnSAId1.HasValue
            && !(await _dbContext.CustomerExistOnSalesArrangement(request.CustomerOnSAId1.Value, householdEntity.SalesArrangementId, cancellationToken)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #1 ID {request.CustomerOnSAId1} does not exist in this SA {householdEntity.SalesArrangementId}.");
        if (request.CustomerOnSAId2.HasValue
            && !(await _dbContext.CustomerExistOnSalesArrangement(request.CustomerOnSAId2.Value, householdEntity.SalesArrangementId, cancellationToken)))
            throw new CisNotFoundException(16020, $"CustomerOnSA #2 ID {request.CustomerOnSAId2} does not exist in this SA {householdEntity.SalesArrangementId}.");

        householdEntity.CustomerOnSAId1 = request.CustomerOnSAId1;
        householdEntity.CustomerOnSAId2 = request.CustomerOnSAId2;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly HouseholdServiceDbContext _dbContext;
    
    public LinkCustomerOnSAToHouseholdHandler(HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
