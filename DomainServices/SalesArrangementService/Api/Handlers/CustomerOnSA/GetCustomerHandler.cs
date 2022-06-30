using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.SalesArrangementService.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

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
        var obligationsBin = await _dbContext.CustomersObligations
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .Select(t => t.DataBin)
            .FirstOrDefaultAsync(cancellation);

        if (obligationsBin is not null)
        {
            var obligations = Contracts.ObligationsCollection.Parser.ParseFrom(obligationsBin);
            customerInstance.Obligations.AddRange(obligations.Items);
        }

        // incomes
        var list = await _dbContext.CustomersIncomes
           .AsNoTracking()
           .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
           .Select(CustomerOnSAServiceRepositoryExpressions.Income())
           .ToListAsync(cancellation);
        customerInstance.Incomes.AddRange(list);

        return customerInstance;
    }
    
    private readonly SalesArrangementServiceDbContext _dbContext;
    
    public GetCustomerHandler(
        SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}