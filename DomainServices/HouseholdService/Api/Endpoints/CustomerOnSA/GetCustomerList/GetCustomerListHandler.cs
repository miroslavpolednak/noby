using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomerList;

internal sealed class GetCustomerListHandler
    : IRequestHandler<GetCustomerListRequest, GetCustomerListResponse>
{
    public async Task<GetCustomerListResponse> Handle(GetCustomerListRequest request, CancellationToken cancellationToken)
    {
        var customers = await _dbContext.Customers
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceExpressions.CustomerDetail())
            .ToListAsync(cancellationToken);
        var ids = customers.Select(t => t.CustomerOnSAId).ToList();

        var identities = await _dbContext.CustomersIdentities
            .Where(t => ids.Contains(t.CustomerOnSAId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        customers.ForEach(t =>
        {
            t.CustomerIdentifiers.AddRange(
                identities
                    .Where(x => x.CustomerOnSAId == t.CustomerOnSAId)
                    .Select(x => new Identity(x.IdentityId, x.IdentityScheme))
            );
        });

        var model = new GetCustomerListResponse();
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