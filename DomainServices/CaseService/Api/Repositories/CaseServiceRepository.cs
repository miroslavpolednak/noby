using Microsoft.EntityFrameworkCore;

namespace DomainServices.CaseService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CaseServiceRepository
{
    private readonly CaseServiceDbContext _dbContext;

    public CaseServiceRepository(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task<Entities.CaseInstance> getCaseEntity(long caseId)
        => await _dbContext.CaseInstances.FindAsync(caseId) ?? throw new CIS.Core.Exceptions.CisNotFoundException(13000, $"Case #{caseId} not found");

    public async Task<bool> IsExistingCase(long caseId)
        => await _dbContext.CaseInstances.AnyAsync(t => t.CaseId == caseId);

    public async Task CreateCase(Entities.CaseInstance entity)
    {
        _dbContext.CaseInstances.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Entities.CaseInstance> GetCaseDetail(long caseId)
        => await getCaseEntity(caseId);

    public async Task<Contracts.SearchCasesResponse> GetCaseList(CIS.Infrastructure.gRPC.CisTypes.PaginationRequest pagination, int userId, int? state, string? searchTerm)
    {
        var query = _dbContext.CaseInstances.AsNoTracking().Where(t => t.UserId == userId);
        if (state.HasValue)
            query = query.Where(t => t.State == state.Value);

        var result = new Contracts.SearchCasesResponse()
        {
            Pagination = pagination.CreateResponse(await query.CountAsync())
        };
        result.CaseInstances.AddRange(await query
            .Take(pagination.PageSize)
            .Skip(pagination.PageSize * (pagination.RecordOffset - 1))
            .Select(t => new Contracts.CaseModel
            {
                CaseId = t.CaseId,
                State = t.State,
                ActionRequired = t.IsActionRequired,
                ContractNumber = t.ContractNumber ?? "",
                DateOfBirthNaturalPerson = t.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = t.FirstNameNaturalPerson,
                Name = t.Name,
                Customer = !t.CustomerIdentityId.HasValue ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(t.CustomerIdentityId, t.CustomerIdentityScheme),
                ProductInstanceType = t.ProductInstanceType,
                TargetAmount = t.TargetAmount,
                UserId = t.UserId,
                Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
            }).ToListAsync()
        );

        return result;
    }

    public async Task LinkOwnerToCase(long caseId, int userId)
    {
        (await getCaseEntity(caseId)).UserId = userId;
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateCaseData(long caseId, string contractNumber)
    {
        var entity = await getCaseEntity(caseId);
        entity.ContractNumber = contractNumber;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCaseState(long caseId, int state)
    {
        (await getCaseEntity(caseId)).State = state;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCaseCustomer(long caseId, int? identityId, CIS.Core.IdentitySchemes? scheme, string firstName, string name, DateOnly? birthDate)
    {
        var entity = await getCaseEntity(caseId);
        
        entity.DateOfBirthNaturalPerson = birthDate;
        entity.Name = name;
        entity.FirstNameNaturalPerson = firstName;
        entity.CustomerIdentityId = identityId;
        entity.CustomerIdentityScheme = scheme;

        await _dbContext.SaveChangesAsync();
    }
}
