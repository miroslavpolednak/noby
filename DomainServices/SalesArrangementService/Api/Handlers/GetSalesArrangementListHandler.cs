using DomainServices.SalesArrangementService.Api.Repositories;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementsListHandler
    : IRequestHandler<Dto.GetSalesArrangementListMediatrRequest, GetSalesArrangementListResponse>
{
    public async Task<GetSalesArrangementListResponse> Handle(Dto.GetSalesArrangementListMediatrRequest request, CancellationToken cancellation)
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
