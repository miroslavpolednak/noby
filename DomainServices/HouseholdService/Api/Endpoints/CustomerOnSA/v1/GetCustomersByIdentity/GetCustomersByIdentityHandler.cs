using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.GetCustomersByIdentity;

internal sealed class GetCustomersByIdentityHandler(HouseholdServiceDbContext _dbContext)
    : IRequestHandler<GetCustomersByIdentityRequest, GetCustomersByIdentityResponse>
{
    public async Task<GetCustomersByIdentityResponse> Handle(GetCustomersByIdentityRequest request, CancellationToken cancellationToken)
    {
        var identifier = request.CustomerIdentifier;
        var identities = await _dbContext.CustomersIdentities
            .Where(t => t.IdentityId == identifier.IdentityId && (byte)t.IdentityScheme == (byte)identifier.IdentityScheme)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var customerOnSaIds = identities.Select(i => i.CustomerOnSAId).ToHashSet();

        var customers = await _dbContext.Customers
            .Where(t => customerOnSaIds.Contains(t.CustomerOnSAId))
            .AsNoTracking()
            .Select(CustomerOnSAServiceExpressions.CustomerDetail())
            .ToListAsync(cancellationToken);

        var response = new GetCustomersByIdentityResponse();
        response.Customers.AddRange(customers);
        return response;
    }
}