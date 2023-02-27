using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.Household.LinkCustomerOnSAToHousehold;

internal sealed class LinkCustomerOnSAToHouseholdHandler
    : IRequestHandler<LinkCustomerOnSAToHouseholdRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(LinkCustomerOnSAToHouseholdRequest request, CancellationToken cancellationToken)
    {
        // domacnost
        var householdEntity = await _dbContext.Households.FindAsync(new object[] { request.HouseholdId }, cancellationToken);

        householdEntity!.CustomerOnSAId1 = request.CustomerOnSAId1;
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
