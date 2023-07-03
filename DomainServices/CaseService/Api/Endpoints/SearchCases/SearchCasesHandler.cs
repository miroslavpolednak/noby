using CIS.Core.Types;
using CIS.Infrastructure.Data;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.CaseService.Api.Endpoints.SearchCases;

internal sealed class SearchCasesHandler
    : IRequestHandler<SearchCasesRequest, SearchCasesResponse>
{
    /// <summary>
    /// Seznam Case s moznosti strankovani. Vetsinou pro daneho uzivatele.
    /// </summary>
    public async Task<SearchCasesResponse> Handle(SearchCasesRequest request, CancellationToken cancellation)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper);

        // dotaz do DB
        var query = getQuery(request, paginable);

        // celkem nalezeno
        int recordsTotalSize = await query.AsNoTracking().CountAsync(cancellation);

        // seznam case
        var data = await query
            .Skip(paginable.RecordOffset)
            .Take(paginable.PageSize)
            .AsNoTracking()
            .Select(CaseServiceDatabaseExpressions.CaseDetail()
        ).ToListAsync(cancellation);

        // get active tasks - nejde delat pres EF kvuli Grpc kolekci
        var caseIds = data.Select(t => t.CaseId).ToArray();
        var tasksCollection = await _dbContext.ActiveTasks
            .Where(t => caseIds.Contains(t.CaseId))
            .AsNoTracking()
            .Select(t => new
            {
                t.CaseId,
                t.TaskProcessId,
                t.TaskTypeId,
                t.TaskIdSb
            })
            .ToListAsync(cancellation);

        // rozsekat na jednotlive cases
        data.ForEach(t => t.Tasks.AddRange(
            tasksCollection
                .Where(x => x.CaseId == t.CaseId)
                .Select(x => new ActiveTask 
                { 
                    TaskId = x.TaskProcessId, 
                    TaskTypeId = x.TaskTypeId 
                })
                .ToList()
        ));

        var result = new SearchCasesResponse()
        {
            Pagination = new CIS.Infrastructure.gRPC.CisTypes.PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, recordsTotalSize)
        };
        result.Cases.AddRange(data);

        return result;
    }

    private IQueryable<Database.Entities.Case> getQuery(SearchCasesRequest request, Paginable paginable)
    {
        // base query
        var query = _dbContext.Cases
            .AsNoTracking()
            .Where(t => t.OwnerUserId == request.CaseOwnerUserId);

        // omezeni na state
        if (request.State?.Any() ?? false)
            query = query.Where(t => request.State.Contains(t.State));
        // hledani podle retezce
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            if (request.SearchTerm.Length < 8 && int.TryParse(request.SearchTerm, out int searchCaseId))
                query = query.Where(t => t.CaseId == searchCaseId);
            else
                query = query.Where(t => t.Name.Contains(request.SearchTerm) || t.ContractNumber!.Contains(request.SearchTerm));
        }

        return adjustPaging(query, paginable);
    }

    private static IQueryable<Database.Entities.Case> adjustPaging(IQueryable<Database.Entities.Case>  query, Paginable paginable)
    {
        if (paginable.HasSorting)
            query = query.ApplyOrderBy(new[] { Tuple.Create(paginable.Sorting!.First().Field, paginable.Sorting!.First().Descending) });
        else
            query = query.OrderByDescending(t => t.StateUpdateTime);

        return query;
    }

    // povolena pole pro sortovani
    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new("StateUpdatedOn", "StateUpdateTime")
    };

    private readonly CaseServiceDbContext _dbContext;

    public SearchCasesHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
