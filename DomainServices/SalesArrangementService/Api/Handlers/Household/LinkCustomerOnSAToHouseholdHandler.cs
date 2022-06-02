using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class LinkCustomerOnSAToHouseholdHandler
    : IRequestHandler<Dto.LinkCustomerOnSAToHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkCustomerOnSAToHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        // domacnost
        var householdEntity = await _dbContext.Households
            .FirstOrDefaultAsync(t => t.HouseholdId == request.Request.HouseholdId, cancellation) 
            ?? throw new CisNotFoundException(16022, $"Household ID {request.Request.HouseholdId} does not exist.");

        // overeni existence customeru
        await _repository.CheckCustomers(householdEntity.SalesArrangementId, request.Request.CustomerOnSAId1, request.Request.CustomerOnSAId2, cancellation);

        householdEntity.CustomerOnSAId1 = request.Request.CustomerOnSAId1;
        householdEntity.CustomerOnSAId2 = request.Request.CustomerOnSAId2;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly Repositories.HouseholdRepository _repository;
    
    public LinkCustomerOnSAToHouseholdHandler(
        Repositories.HouseholdRepository repository,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _repository = repository;
        _dbContext = dbContext;
    }
}
