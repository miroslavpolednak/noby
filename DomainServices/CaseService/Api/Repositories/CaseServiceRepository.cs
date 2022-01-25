using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CaseServiceRepository
{
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
            .Select(CaseServiceRepositoryExtensions.CaseDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CIS.Core.Exceptions.CisNotFoundException(13000, $"Case #{caseId} not found");

    public async Task<Contracts.SearchCasesResponse> GetCaseList(CIS.Infrastructure.gRPC.CisTypes.PaginationRequest pagination, int userId, int? state, string? searchTerm, CancellationToken cancellation)
    {
        var query = _dbContext.CaseInstances.AsNoTracking().Where(t => t.OwnerUserId == userId);
        // omezeni na state
        if (state.HasValue)
            query = query.Where(t => t.State == state.Value);
        // hledani podle retezce
        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(t => t.Name.Contains(searchTerm) || t.ContractNumber == searchTerm);
        
        // razeni
        if (pagination.Sorting is not null && pagination.Sorting.Any())
            query = query.ApplyOrderBy(new[] { Tuple.Create(pagination.Sorting.First().Field, pagination.Sorting.First().Descending) });
        else
            query = query.OrderByDescending(t => t.CreatedTime);

        var result = new Contracts.SearchCasesResponse()
        {
            Pagination = pagination.CreateResponse(await query.AsNoTracking().CountAsync(cancellation))
        };
        result.CaseInstances.AddRange(
            await query
                .Skip(pagination.PageSize * (pagination.RecordOffset - 1))
                .Take(pagination.PageSize)
                .AsNoTracking()
                .Select(CaseServiceRepositoryExtensions.CaseDetail()
            ).ToListAsync(cancellation)
        );

        return result;
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
        entity.ProductInstanceType = data.ProductInstanceType;

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
        => await _dbContext.CaseInstances.FindAsync(new object[] { caseId }, cancellation) ?? throw new CIS.Core.Exceptions.CisNotFoundException(13000, $"Case #{caseId} not found");

    private readonly CaseServiceDbContext _dbContext;

    public CaseServiceRepository(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
