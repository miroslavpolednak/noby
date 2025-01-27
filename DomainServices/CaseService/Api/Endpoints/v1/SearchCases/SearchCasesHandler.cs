﻿using CIS.Core.Types;
using CIS.Infrastructure.Data;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace DomainServices.CaseService.Api.Endpoints.v1.SearchCases;

internal sealed class SearchCasesHandler(
    TimeProvider _timeProvider,
    CaseServiceDbContext _dbContext)
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
            .EnsureAndTranslateSortFields(_sortingMapper);

        // dotaz do DB
        var query = getQuery(request, paginable);

        // celkem nalezeno
        int recordsTotalSize = await query.AsNoTracking().CountAsync(cancellation);

        // seznam case
        var data = await query
            .Skip(paginable.RecordOffset)
            .Take(paginable.PageSize)
            .AsNoTracking()
            .Select(DatabaseExpressions.CaseDetail()
        ).ToListAsync(cancellation);

        // get active tasks - nejde delat pres EF kvuli Grpc kolekci
        var caseIds = data.Select(t => t.CaseId).ToArray();
        var tasksCollection = await _dbContext.ActiveTasks
            .Where(t => caseIds.Contains(t.CaseId))
            .AsNoTracking()
            .Select(t => new
            {
                t.CaseId,
                t.TaskId,
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
                    TaskId = x.TaskId,
                    TaskIdSb = x.TaskIdSb,
                    TaskTypeId = x.TaskTypeId
                })
                .ToList()
        ));

        var result = new SearchCasesResponse()
        {
            Pagination = new SharedTypes.GrpcTypes.PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, recordsTotalSize)
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
        {
            query = query.Where(t => request.State.Contains(t.State));
        }

        if (request.StateUpdatedTimeLimitInDays.HasValue)
        {
            DateTime? dexp = request.StateUpdatedTimeLimitInDays.HasValue ? _timeProvider.GetLocalNow().Date.AddDays(request.StateUpdatedTimeLimitInDays.Value * -1) : null;
            query = query.Where(t => t.State != (int)EnumCaseStates.InAdministration || t.StateUpdateTime >= dexp);
        }

        // hledani podle retezce
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            if (long.TryParse(request.SearchTerm, out long searchCaseId)) // muze to byt hledani podle caseId
            {
                query = query.Where(t => t.Name.Contains(request.SearchTerm) || t.ContractNumber!.Contains(request.SearchTerm) || t.CaseId == searchCaseId);
            }
            else
            {
                query = query.Where(t => t.Name.Contains(request.SearchTerm) || t.ContractNumber!.Contains(request.SearchTerm));
            }
        }

        return adjustPaging(query, paginable);
    }

    private static IQueryable<Database.Entities.Case> adjustPaging(IQueryable<Database.Entities.Case> query, Paginable paginable)
    {
        if (paginable.HasSorting)
            query = query.ApplyOrderBy([Tuple.Create(paginable.Sorting!.First().Field, paginable.Sorting!.First().Descending)]);
        else
            query = query.OrderByDescending(t => t.StateUpdateTime);

        return query;
    }

    // povolena pole pro sortovani
    private static readonly List<Paginable.MapperField> _sortingMapper =
    [
        new("StateUpdatedOn", "StateUpdateTime")
    ];
}
