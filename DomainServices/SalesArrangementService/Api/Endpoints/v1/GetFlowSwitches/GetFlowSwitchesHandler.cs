using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetFlowSwitches;

internal sealed class GetFlowSwitchesHandler
    : IRequestHandler<__SA.GetFlowSwitchesRequest, __SA.GetFlowSwitchesResponse>
{
    public async Task<__SA.GetFlowSwitchesResponse> Handle(__SA.GetFlowSwitchesRequest request, CancellationToken cancellation)
    {
        var flowSwitches = await _dbContext.FlowSwitches
            .AsNoTracking()
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .Select(t => new __SA.FlowSwitch
            {
                FlowSwitchId = t.FlowSwitchId,
                Value = t.Value
            })
            .ToListAsync(cancellation);

        // neni nastaveny zadny flow switch / kontrola jestli vubec existuje SA
        if (flowSwitches is null || flowSwitches.Count == 0)
        {
            if (!(await _dbContext.SalesArrangements.AnyAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)))
            {
                throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);
            }
        }

        var response = new __SA.GetFlowSwitchesResponse();
        response.FlowSwitches.AddRange(flowSwitches);
        return response;
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public GetFlowSwitchesHandler(Database.SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
