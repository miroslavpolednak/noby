﻿using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance.GetOfferGuaranteeDateToCheck;

internal sealed class GetOfferGuaranteeDateToCheckHandler(
    SalesArrangementServiceDbContext _dbContext, 
    TimeProvider _dateTime)
		: IRequestHandler<GetOfferGuaranteeDateToCheckRequest, GetOfferGuaranteeDateToCheckResponse>
{
    public async Task<GetOfferGuaranteeDateToCheckResponse> Handle(GetOfferGuaranteeDateToCheckRequest request, CancellationToken cancellationToken)
    {
        var flowSwitches = await _dbContext.FlowSwitches
            .Include(f => f.SalesArrangement)
            .Where(f =>
                f.FlowSwitchId == (int)FlowSwitches.IsOfferGuaranteed
                && f.Value
                && _saStates.Contains(f.SalesArrangement.State)
                && f.SalesArrangement.OfferGuaranteeDateTo != null
                && f.SalesArrangement.OfferGuaranteeDateTo!.Value.Date < _dateTime.GetLocalNow().Date)
            .Select(t => new GetOfferGuaranteeDateToCheckResponse.Types.GetOfferGuaranteeDateToCheckItem 
            { 
                SalesArrangementId = t.SalesArrangementId, 
                CaseId = t.SalesArrangement.CaseId
            })
            .ToListAsync(cancellationToken);

        GetOfferGuaranteeDateToCheckResponse response = new();
        response.Items.AddRange(flowSwitches);
        return response;
    }

    private static int[] _saStates = 
    [ 
        (int)EnumSalesArrangementStates.InProgress,
        (int)EnumSalesArrangementStates.NewArrangement, 
        (int)EnumSalesArrangementStates.InSigning, 
        (int)EnumSalesArrangementStates.ToSend 
    ];
}
