﻿using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementList;

internal sealed class GetSalesArrangementsListHandler(
    SalesArrangementServiceDbContext _dbContext, 
    CaseService.Clients.v1.ICaseServiceClient _caseService)
		: IRequestHandler<GetSalesArrangementListRequest, GetSalesArrangementListResponse>
{
    public async Task<GetSalesArrangementListResponse> Handle(GetSalesArrangementListRequest request, CancellationToken cancellation)
    {
        var list = await _dbContext.SalesArrangements
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId)
            .OrderByDescending(t => t.SalesArrangementId)
            .Select(DatabaseExpressions.SalesArrangementDetail())
            .ToListAsync(cancellation);

        // kontrola na existenci case - kvuli efektivite jen pokud se nevrati zadny SA
        if (list.Count == 0)
        {
            await _caseService.ValidateCaseId(request.CaseId, true, cancellation);
        }
        
        GetSalesArrangementListResponse model = new();
        model.SalesArrangements.AddRange(list);
        return model;
    }
}
