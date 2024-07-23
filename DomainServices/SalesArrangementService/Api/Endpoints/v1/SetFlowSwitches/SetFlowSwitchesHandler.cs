﻿    using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SetFlowSwitches;

internal sealed class SetFlowSwitchesHandler(Database.SalesArrangementServiceDbContext _dbContext)
		: IRequestHandler<__SA.SetFlowSwitchesRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(__SA.SetFlowSwitchesRequest request, CancellationToken cancellation)
    {
        var usedFlowSwitches = request.FlowSwitches.Select(t => t.FlowSwitchId).ToArray();

        var flowSwitches = await _dbContext
            .FlowSwitches
            .Where(t => t.SalesArrangementId == request.SalesArrangementId && usedFlowSwitches.Contains(t.FlowSwitchId))
            .ToListAsync(cancellation);

        foreach (var requestSwitch in request.FlowSwitches)
        {
            var dbSwitch = flowSwitches.FirstOrDefault(t => t.FlowSwitchId == requestSwitch.FlowSwitchId);

            // pridani noveho switche
            if (dbSwitch is null && requestSwitch.Value.HasValue)
            {
                _dbContext.FlowSwitches.Add(new Database.Entities.FlowSwitch
                {
                    SalesArrangementId = request.SalesArrangementId,
                    FlowSwitchId = requestSwitch.FlowSwitchId,
                    Value = requestSwitch.Value.Value
                });
            }
            // kompletne smazat klapku
            else if (dbSwitch is not null && !requestSwitch.Value.HasValue)
            {
                _dbContext.FlowSwitches.Remove(dbSwitch);
            }
            // update Value
            else if (dbSwitch is not null && requestSwitch.Value != dbSwitch.Value)
            {
                dbSwitch.Value = requestSwitch.Value!.Value;
            }
        }

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
