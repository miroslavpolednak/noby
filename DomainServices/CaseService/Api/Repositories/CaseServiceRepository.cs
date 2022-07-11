using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class CaseServiceRepository
{
    public async Task<List<(int State, int Count)>> GetCounts(int userId, CancellationToken cancellation)
        => (await _dbContext.Cases
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

    public async Task DeleteCase(long caseId, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);
        _dbContext.Cases.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task CreateCase(Entities.Case entity, CancellationToken cancellation)
    {
        _dbContext.Cases.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task<Contracts.Case> GetCaseDetail(long caseId, CancellationToken cancellation)
        => await _dbContext.Cases
            .Where(t => t.CaseId == caseId)
            .AsNoTracking()
            .Select(CaseServiceRepositoryExpressions.CaseDetail())
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Case #{caseId} not found");

    public async Task<(int RecordsTotalSize, List<Contracts.Case> CaseInstances)> GetCaseList(
        CIS.Core.Types.Paginable paginable, 
        int userId, 
        List<int>? state, 
        string? searchTerm, 
        CancellationToken cancellation)
    {
        // base query
        var query = _dbContext.Cases.AsNoTracking().Where(t => t.OwnerUserId == userId);

        // omezeni na state
        if (state is not null && state.Any())
            query = query.Where(t => state.Contains(t.State));
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
            .Skip(paginable.RecordOffset)
            .Take(paginable.PageSize)
            .AsNoTracking()
            .Select(CaseServiceRepositoryExpressions.CaseDetail()
        ).ToListAsync(cancellation);

        // get active tasks - nejde delat pres EF kvuli Grpc kolekci
        var caseIds = data.Select(t => t.CaseId).ToArray();
        var tasksCollection = await _dbContext.ActiveTasks
            .Where(t => caseIds.Contains(t.CaseId))
            .AsNoTracking()
            .Select(t => new
            {
                CaseId = t.CaseId,
                TaskProcessId = t.TaskProcessId,
                TaskTypeId = t.TaskTypeId
            })
            .ToListAsync(cancellation);

        // rozsekat na jednotlive cases
        data.ForEach(t => t.Tasks.AddRange(
            tasksCollection
                .Where(x => x.CaseId == t.CaseId)
                .Select(x => new Contracts.ActiveTask { TaskProcessId = x.TaskProcessId, TypeId = x.TaskTypeId })
                .ToList()
        ));

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
        entity.ProductTypeId = data.ProductTypeId;

        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpdateCaseState(long caseId, int state, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);

        entity.State = state;
        entity.StateUpdateTime = _dateTime.Now;

        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpdateCaseCustomer(long caseId, Contracts.CustomerData customer, CancellationToken cancellation)
    {
        var entity = await getCaseEntity(caseId, cancellation);
        
        entity.DateOfBirthNaturalPerson = customer.DateOfBirthNaturalPerson;
        entity.Name = customer.Name;
        entity.FirstNameNaturalPerson = customer.FirstNameNaturalPerson;
        entity.CustomerIdentityId = customer.Identity?.IdentityId;
        entity.CustomerIdentityScheme = (CIS.Foms.Enums.IdentitySchemes)Convert.ToInt32(customer.Identity?.IdentityScheme, System.Globalization.CultureInfo.InvariantCulture);

        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task ReplaceActiveTasks(long caseId, Contracts.ActiveTask[] tasks, CancellationToken cancellation)
    {
        var taskProcessIds = tasks.Select(i => i.TaskProcessId);

        var entities = _dbContext.ActiveTasks.Where(i => i.CaseId == caseId);
        var entitiesIds = entities.Select(i => i.TaskProcessId);

        var idsToAdd = taskProcessIds.Where(id => !entitiesIds.Contains(id)).ToList();
        var idsToRemove = entitiesIds.Where(id => !taskProcessIds.Contains(id)).ToList();
        var idsToUpdate = entitiesIds.Where(id => !idsToAdd.Contains(id) && !idsToRemove.Contains(id)).ToList();

        // remove
        if (idsToRemove.Any())
        {
            _dbContext.ActiveTasks.RemoveRange(entities.Where(e => idsToRemove.Contains(e.TaskProcessId)));
        }

        // add
        if (idsToAdd.Any())
        {
            _dbContext.ActiveTasks.AddRange(
            tasks.Where(t => idsToAdd.Contains(t.TaskProcessId)).Select(t => new Entities.ActiveTask { CaseId = caseId, TaskProcessId = t.TaskProcessId, TaskTypeId = t.TypeId })
            );
        }

        // update
        if (idsToUpdate.Any())
        {
            var tasksToUpdateById = tasks.Where(t => idsToUpdate.Contains(t.TaskProcessId)).ToDictionary(t => t.TaskProcessId);
            entities.Where(e => idsToUpdate.Contains(e.TaskProcessId)).ToList().ForEach(e =>
            {
                e.TaskTypeId = tasksToUpdateById[e.TaskProcessId].TypeId;
            });
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }

    private async Task<Entities.Case> getCaseEntity(long caseId, CancellationToken cancellation)
        => await _dbContext.Cases.FindAsync(new object[] { caseId }, cancellation) ?? throw new CisNotFoundException(13000, $"Case #{caseId} not found");

    private readonly CaseServiceDbContext _dbContext;
    private readonly CIS.Core.IDateTime _dateTime;

    public CaseServiceRepository(CaseServiceDbContext dbContext, CIS.Core.IDateTime dateTime)
    {
        _dateTime = dateTime;
        _dbContext = dbContext;
    }
}
