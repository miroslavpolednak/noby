﻿using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CaseServiceRepository
{
    public async Task<List<(int State, int Count)>> GetCounts(int userId, CancellationToken cancellation)
        => (await _dbContext.CaseInstances
            .Where(t => t.OwnerUserId == userId)
            .GroupBy(t => t.State)
            .AsNoTracking()
            .Select(t => new { State = t.Key, Count = t.Count() })
            .ToListAsync(cancellation))
            .Select(t => (State: t.State, Count: t.Count)).ToList();

    public async Task EnsureExistingCase(long caseId, CancellationToken cancellation)
    {
        await getCaseEntity(caseId, cancellation);
    }

    public async Task CreateCase(Entities.CaseInstance entity, CancellationToken cancellation)
    {
        _dbContext.CaseInstances.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task<Contracts.Case> GetCaseDetail(long caseId, CancellationToken cancellation)
        => await _dbContext.CaseInstances
            .Where(t => t.CaseId == caseId)
            .AsNoTracking()
            .Select(CaseServiceRepositoryExpressions.CaseDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Case #{caseId} not found");

    public async Task<(int RecordsTotalSize, List<Contracts.Case> CaseInstances)> GetCaseList(CIS.Core.Types.Paginable paginable, int userId, int? state, string? searchTerm, CancellationToken cancellation)
    {
        // base query
        var query = _dbContext.CaseInstances.AsNoTracking().Where(t => t.OwnerUserId == userId);

        // omezeni na state
        if (state.HasValue)
            query = query.Where(t => t.State == state.Value);
        // hledani podle retezce
        if (!string.IsNullOrEmpty(searchTerm))
        {
            if (searchTerm.Length < 8 && int.TryParse(searchTerm, out int searchCaseId))
                query = query.Where(t => t.CaseId == searchCaseId);
            else if ((searchTerm.Length == 10 || searchTerm.Length == 8) && decimal.TryParse(searchTerm, out _))
                query = query.Where(t => t.ContractNumber == searchTerm);
            else
                query = query.Where(t => t.Name.Contains(searchTerm));
        }
        
        // razeni
        if (paginable.HasSorting)
            query = query.ApplyOrderBy(new[] { Tuple.Create(paginable.Sorting!.First().Field, paginable.Sorting!.First().Descending) });
        else
            query = query.OrderByDescending(t => t.StateUpdateTime);

        // celkem nalezeno
        int recordsTotalSize = await query.AsNoTracking().CountAsync(cancellation);

        // seznam case
        var data = await query
            .Skip(paginable.PageSize * (paginable.RecordOffset - 1))
            .Take(paginable.PageSize)
            .AsNoTracking()
            .Select(CaseServiceRepositoryExpressions.CaseDetail()
        ).ToListAsync(cancellation);

        return (recordsTotalSize, data);
    }

    public async Task LinkOwnerToCase(long caseId, int ownerUserId, string ownerName, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);

        entity.OwnerUserId = ownerUserId;
        entity.OwnerUserName = ownerName;
        
        await _dbContext.SaveChangesAsync(cancellation);
    }
    
    public async Task UpdateCaseData(long caseId, Contracts.CaseData data, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);
        
        entity.ContractNumber = data.ContractNumber;
        entity.TargetAmount = data.TargetAmount;
        entity.ProductInstanceTypeId = data.ProductInstanceTypeId;

        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpdateCaseState(long caseId, int state, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);

        entity.State = state;
        entity.StateUpdateTime = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpdateCaseCustomer(long caseId, Contracts.CustomerData customer, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);
        
        entity.DateOfBirthNaturalPerson = customer.DateOfBirthNaturalPerson;
        entity.Name = customer.Name;
        entity.FirstNameNaturalPerson = customer.FirstNameNaturalPerson;
        entity.CustomerIdentityId = customer.Identity?.IdentityId;
        entity.CustomerIdentityScheme = (CIS.Core.IdentitySchemes)Convert.ToInt32(customer.Identity?.IdentityScheme);

        await _dbContext.SaveChangesAsync();
    }

    private async Task<Entities.CaseInstance> getCaseEntity(long caseId, CancellationToken cancellation)
        => await _dbContext.CaseInstances.FindAsync(new object[] { caseId }, cancellation) ?? throw new CisNotFoundException(13000, $"Case #{caseId} not found");

    private readonly CaseServiceDbContext _dbContext;

    public CaseServiceRepository(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
