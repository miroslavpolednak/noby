using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA;

internal class GetCustomerHandler
    : IRequestHandler<Dto.GetCustomerMediatrRequest, Contracts.CustomerOnSA>
{
    public async Task<Contracts.CustomerOnSA> Handle(Dto.GetCustomerMediatrRequest request, CancellationToken cancellation)
    {
        var customerInstance = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceRepositoryExpressions.CustomerDetail())
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
            .Select(CustomerOnSAServiceRepositoryExpressions.Obligation())
            .ToListAsync(cancellation);
        customerInstance.Obligations.AddRange(obligations);

        // incomes
        var list = await _dbContext.CustomersIncomes
           .AsNoTracking()
           .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
           .Select(CustomerOnSAServiceRepositoryExpressions.Income())
           .ToListAsync(cancellation);
        customerInstance.Incomes.AddRange(list);

        return customerInstance;
    }
    
    private readonly HouseholdServiceDbContext _dbContext;
    
    public GetCustomerHandler(
        HouseholdServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}