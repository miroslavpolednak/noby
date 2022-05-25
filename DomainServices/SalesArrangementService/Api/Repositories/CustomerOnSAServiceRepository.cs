using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.SalesArrangementService.Api.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CustomerOnSAServiceRepository
{
    public async Task<List<Contracts.CustomerOnSA>> GetList(int salesArrangementId, CancellationToken cancellation)
    {
        var model = await _dbContext.Customers
            .Where(t => t.SalesArrangementId == salesArrangementId)
            .AsNoTracking()
            .Select(CustomerOnSAServiceRepositoryExpressions.CustomerDetail())
            .ToListAsync(cancellation);
        var ids = model.Select(t => t.CustomerOnSAId).ToList();
        
        var identities = await _dbContext.CustomersIdentities
            .Where(t => ids.Contains(t.CustomerOnSAId))
            .AsNoTracking()
            .ToListAsync(cancellation);
        
        model.ForEach(t =>
        {
            t.CustomerIdentifiers.AddRange(identities.Where(x => x.CustomerOnSAId == t.CustomerOnSAId).Select(x => new Identity(x.IdentityId, x.IdentityScheme)));
        });

        return model;
    }

    public async Task<int> DeleteCustomer(int customerOnSAId, CancellationToken cancellation)
    {
        var entity = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {customerOnSAId} does not exist.");

        _dbContext.Customers.Remove(entity);

        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.CustomerOnSAIdentity WHERE CustomerOnSAId={customerOnSAId}", cancellation);
        
        await _dbContext.SaveChangesAsync(cancellation);

        return entity.SalesArrangementId;
    }
    
    private readonly SalesArrangementServiceDbContext _dbContext;

    public CustomerOnSAServiceRepository(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}