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
            .Select(SalesArrangementServiceRepositoryExpressions.SalesArrangementDetail())
            .ToListAsync(cancellation);

        GetSalesArrangementListResponse model = new();
        model.SalesArrangements.AddRange(list);
        return model;
    }

    private readonly SalesArrangementServiceDbContext _dbContext;

    public GetSalesArrangementsListHandler(SalesArrangementServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
