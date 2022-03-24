using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.Household;

internal class LinkCustomerOnSAToHouseholdHandler
    : IRequestHandler<Dto.LinkCustomerOnSAToHouseholdMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkCustomerOnSAToHouseholdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkCustomerOnSAToHouseholdHandler), request.Request.HouseholdId);

        // domacnost
        var householdEntity = await _dbContext.Households
            .FirstOrDefaultAsync(t => t.HouseholdId == request.Request.HouseholdId, cancellation) 
            ?? throw new CisNotFoundException(16022, $"Household ID {request.Request.HouseholdId} does not exist.");

        // overeni existence customeru
        if (request.Request.CustomerOnSAId1.HasValue && !_dbContext.Customers.Any(t => t.CustomerOnSAId == request.Request.CustomerOnSAId1))
            throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.Request.CustomerOnSAId1} does not exist.");
        if (request.Request.CustomerOnSAId2.HasValue && !_dbContext.Customers.Any(t => t.CustomerOnSAId == request.Request.CustomerOnSAId2))
            throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.Request.CustomerOnSAId2} does not exist.");

        householdEntity.CustomerOnSAId1 = request.Request.CustomerOnSAId1;
        householdEntity.CustomerOnSAId2 = request.Request.CustomerOnSAId2;

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<LinkCustomerOnSAToHouseholdHandler> _logger;

    public LinkCustomerOnSAToHouseholdHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<LinkCustomerOnSAToHouseholdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
