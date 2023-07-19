using CIS.Foms.Enums;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseCounts;

internal sealed class GetCaseCountsHandler
    : IRequestHandler<GetCaseCountsRequest, GetCaseCountsResponse>
{
    public async Task<GetCaseCountsResponse> Handle(GetCaseCountsRequest request, CancellationToken cancellation)
    {
        // vytahnout data z DB
        var model = (await _dbContext.Cases
            .Where(t => t.OwnerUserId == request.CaseOwnerUserId && !_disallowedStates.Contains(t.State))
            .GroupBy(t => t.State)
            .AsNoTracking()
            .Select(t => new { State = t.Key, Count = t.Count() })
            .ToListAsync(cancellation))
            .Select(t => (t.State, t.Count))
            .ToList();

        var result = new GetCaseCountsResponse();
        result.CaseCounts.AddRange(model
            .Select(t => new GetCaseCountsResponse.Types.CaseCountsItem { Count = t.Count, State = t.State })
            .ToList()
        );

        return result;
    }

    private static int[] _disallowedStates = new[]
    {
        (int)CaseStates.ToBeCancelledConfirmed
    };

    private readonly CaseServiceDbContext _dbContext;

    public GetCaseCountsHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
