using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetProductSalesArrangement;

internal sealed class GetProductSalesArrangementHandler
    : IRequestHandler<__SA.GetProductSalesArrangementRequest, __SA.GetProductSalesArrangementResponse>
{
    public async Task<__SA.GetProductSalesArrangementResponse> Handle(__SA.GetProductSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        var saTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.SalesArrangementCategory == (int)SalesArrangementTypes.Mortgage)
            .Select(t => t.Id)
            .ToList();

        var sa = await _dbContext.SalesArrangements
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId && saTypes.Contains(t.SalesArrangementTypeId))
            .Select(t => new { t.SalesArrangementId, t.OfferId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound);

        return new __SA.GetProductSalesArrangementResponse
        {
            SalesArrangementId = sa.SalesArrangementId,
            OfferId = sa.OfferId
        };
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly ICodebookServiceClient _codebookService;

    public GetProductSalesArrangementHandler(Database.SalesArrangementServiceDbContext dbContext, ICodebookServiceClient codebookService) 
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
