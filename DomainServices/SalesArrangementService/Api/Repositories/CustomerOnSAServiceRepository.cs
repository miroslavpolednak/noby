using CIS.Infrastructure.gRPC.CisTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CustomerOnSAServiceRepository
{
    public async Task<int> CreateCustomer(Entities.CustomerOnSA entity, CancellationToken cancellation)
    {
        _dbContext.Customers.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
        return entity.CustomerOnSAId;
    }

    public async Task<Contracts.CustomerOnSA> GetCustomer(int customerOnSAId, CancellationToken cancellation)
    {
        var model = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceRepositoryExpressions.CustomerDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {customerOnSAId} does not exist.");

        var identities = await _dbContext.CustomersIdentities
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .AsNoTracking()
            .ToListAsync(cancellation);
        model.CustomerIdentifiers.AddRange(identities.Select(t => new Identity(t.Id, t.IdentityScheme)));
        
        return model;
    }

    public async Task DeleteCustomer(int customerOnSAId, CancellationToken cancellation)
    {
        var entity = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {customerOnSAId} does not exist.");

        _dbContext.Customers.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellation);
    }
    
    private readonly CustomerOnSAServiceDbContext _dbContext;
    private readonly CIS.Core.IDateTime _dateTime;

    public CustomerOnSAServiceRepository(CustomerOnSAServiceDbContext dbContext, CIS.Core.IDateTime datetime)
    {
        _dateTime = datetime;
        _dbContext = dbContext;
    }
}