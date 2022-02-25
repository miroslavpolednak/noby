﻿using CIS.Infrastructure.gRPC.CisTypes;
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