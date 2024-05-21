using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetProductSalesArrangement;

internal sealed class GetProductSalesArrangementsHandler(
    Database.SalesArrangementServiceDbContext _dbContext, 
    ICodebookServiceClient _codebookService)
		: IRequestHandler<__SA.GetProductSalesArrangementsRequest, __SA.GetProductSalesArrangementsResponse>
{
    public async Task<__SA.GetProductSalesArrangementsResponse> Handle(__SA.GetProductSalesArrangementsRequest request, CancellationToken cancellationToken)
    {
        var saTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.SalesArrangementCategory == (int)SalesArrangementTypes.Mortgage)
            .Select(t => t.Id)
            .ToList();

        var saList = await _dbContext.SalesArrangements
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId && saTypes.Contains(t.SalesArrangementTypeId))
            .Select(t => new { t.SalesArrangementId, t.OfferId, t.State })
            .ToListAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ProductSalesArrangementNotFound, request.CaseId);

        var result = new __SA.GetProductSalesArrangementsResponse();
        result.SalesArrangements.AddRange(saList.Select(t => new __SA.GetProductSalesArrangementsResponse.Types.SalesArrangement
        {
            SalesArrangementId = t.SalesArrangementId,
            OfferId = t.OfferId,
            State = t.State
        }));

        return result;
    }
}
