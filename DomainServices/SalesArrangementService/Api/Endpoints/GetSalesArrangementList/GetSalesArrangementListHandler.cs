using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementList;

internal sealed class GetSalesArrangementsListHandler
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
        if (!list.Any())
        {
            await _caseService.ValidateCaseId(request.CaseId, true, cancellation);
        }
        
        GetSalesArrangementListResponse model = new();
        model.SalesArrangements.AddRange(list);
        return model;
    }

    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly SalesArrangementServiceDbContext _dbContext;

    public GetSalesArrangementsListHandler(SalesArrangementServiceDbContext dbContext, CaseService.Clients.ICaseServiceClient caseService)
    {
        _dbContext = dbContext;
        _caseService = caseService;
    }
}
