using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Api.Database;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomer;

internal sealed class GetCustomerHandler
    : IRequestHandler<GetCustomerMediatrRequest, Contracts.CustomerOnSA>
{
    public async Task<Contracts.CustomerOnSA> Handle(GetCustomerMediatrRequest request, CancellationToken cancellation)
    {
        var customerInstance = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceExpressions.CustomerDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {request.CustomerOnSAId} does not exist.");

        // identity
        var identities = await _dbContext.CustomersIdentities
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .AsNoTracking()
            .ToListAsync(cancellation);
        customerInstance.CustomerIdentifiers.AddRange(identities.Select(t => new Identity(t.IdentityId, t.IdentityScheme)));

        // obligations
        var obligations = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(CustomerOnSAServiceExpressions.Obligation())
            .ToListAsync(cancellation);
        customerInstance.Obligations.AddRange(obligations);

        // incomes
        var list = await _dbContext.CustomersIncomes
           .AsNoTracking()
           .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
           .Select(CustomerOnSAServiceExpressions.Income())
           .ToListAsync(cancellation);
        customerInstance.Incomes.AddRange(list);

        return customerInstance;
    }

    private readonly HouseholdServiceDbContext _dbContext;

    public GetCustomerHandler(HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}