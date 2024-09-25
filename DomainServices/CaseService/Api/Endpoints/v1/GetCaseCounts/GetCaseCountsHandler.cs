using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetCaseCounts;

internal sealed class GetCaseCountsHandler(
    TimeProvider _timeProvider,
    CaseServiceDbContext _dbContext)
        : IRequestHandler<GetCaseCountsRequest, GetCaseCountsResponse>
{
    public async Task<GetCaseCountsResponse> Handle(GetCaseCountsRequest request, CancellationToken cancellation)
    {
        DateTime? dexp = request.StateUpdatedTimeLimitInDays.HasValue ? _timeProvider.GetLocalNow().Date.AddDays(request.StateUpdatedTimeLimitInDays.Value * -1) : null;

        // vytahnout data z DB
        var model = await _dbContext.Cases
            .Where(t => t.OwnerUserId == request.CaseOwnerUserId)
            .GroupBy(t => t.State)
            .AsNoTracking()
            .Select(t => new 
            { 
                State = t.Key, 
                CountTotal = t.Count(),
                CountLimited = dexp == null ? default(int?) : t.Count(x => x.StateUpdateTime >= dexp),
            })
            .ToListAsync(cancellation);

        var result = new GetCaseCountsResponse();
        result.CaseCounts.AddRange(model
            .Select(t => new GetCaseCountsResponse.Types.CaseCountsItem 
            { 
                CountLimited = t.CountLimited,
                CountTotal = t.CountTotal,
                State = t.State 
            })
            .ToList()
        );

        return result;
    }
}
