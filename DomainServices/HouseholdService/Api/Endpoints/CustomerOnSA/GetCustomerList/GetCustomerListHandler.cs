using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Api.Database;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomerList;

internal sealed class GetCustomerListHandler
    : IRequestHandler<GetCustomerListMediatrRequest, Contracts.GetCustomerListResponse>
{
    public async Task<Contracts.GetCustomerListResponse> Handle(GetCustomerListMediatrRequest request, CancellationToken cancellation)
    {
        var customers = await _dbContext.Customers
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceExpressions.CustomerDetail())
            .ToListAsync(cancellation);
        var ids = customers.Select(t => t.CustomerOnSAId).ToList();

        var identities = await _dbContext.CustomersIdentities
            .Where(t => ids.Contains(t.CustomerOnSAId))
            .AsNoTracking()
            .ToListAsync(cancellation);

        customers.ForEach(t =>
        {
            t.CustomerIdentifiers.AddRange(
                identities
                    .Where(x => x.CustomerOnSAId == t.CustomerOnSAId)
                    .Select(x => new Identity(x.IdentityId, x.IdentityScheme))
            );
        });

        var model = new Contracts.GetCustomerListResponse();
        model.Customers.AddRange(customers);

        _logger.FoundItems(model.Customers.Count);

        return model;
    }

    private readonly HouseholdServiceDbContext _dbContext;
    private readonly ILogger<GetCustomerListHandler> _logger;

    public GetCustomerListHandler(
        HouseholdServiceDbContext dbContext,
        ILogger<GetCustomerListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}