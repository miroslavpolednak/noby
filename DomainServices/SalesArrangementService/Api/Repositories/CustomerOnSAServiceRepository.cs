using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.SalesArrangementService.Api.Repositories.Entities;
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

    public async Task UpdateCustomer(Contracts.UpdateCustomerRequest model, CancellationToken cancellation)
    {
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .Where(t => t.CustomerOnSAId == model.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {model.CustomerOnSAId} does not exist.");
        
        entity.CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)model.CustomerRoleId;
        entity.FirstNameNaturalPerson = model.FirstNameNaturalPerson;
        entity.Name = model.Name;
        entity.DateOfBirthNaturalPerson = model.DateOfBirthNaturalPerson;

        entity.Identities?.RemoveAll(t => true);
        if (model.CustomerIdentifiers is not null && model.CustomerIdentifiers.Any())
        {
            entity.Identities = new List<CustomerOnSAIdentity>();
            entity.Identities.AddRange(model.CustomerIdentifiers.Select(t => new CustomerOnSAIdentity(t, model.CustomerOnSAId)));
        }
        
        await _dbContext.SaveChangesAsync(cancellation);
    }
    
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
            t.CustomerIdentifiers.AddRange(identities.Where(x => x.CustomerOnSAId == t.CustomerOnSAId).Select(x => new Identity(x.Id, x.IdentityScheme)));
        });

        return model;
    }

    public async Task DeleteCustomer(int customerOnSAId, CancellationToken cancellation)
    {
        var entity = await _dbContext.Customers
            .Where(t => t.CustomerOnSAId == customerOnSAId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16020, $"CustomerOnSA ID {customerOnSAId} does not exist.");

        _dbContext.Customers.Remove(entity);

        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.CustomerOnSAIdentity WHERE CustomerOnSAId={customerOnSAId}", cancellation);
        
        await _dbContext.SaveChangesAsync(cancellation);
    }
    
    private readonly SalesArrangementServiceDbContext _dbContext;

    public CustomerOnSAServiceRepository(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}