using DomainServices.CodebookService.Clients;
using Microsoft.EntityFrameworkCore;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetProductSalesArrangement;

internal sealed class GetProductSalesArrangementIdHandler
    : IRequestHandler<__SA.GetProductSalesArrangementIdRequest, __SA.GetProductSalesArrangementIdResponse>
{
    public async Task<__SA.GetProductSalesArrangementIdResponse> Handle(__SA.GetProductSalesArrangementIdRequest request, CancellationToken cancellationToken)
    {
        var saTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(t => t.SalesArrangementCategory == 1)
            .Select(t => t.Id)
            .ToList();

        var sa = await _dbContext.SalesArrangements
            .AsNoTracking()
            .Where(t => saTypes.Contains(t.SalesArrangementTypeId))
            .Select(t => new { t.SalesArrangementId })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound);

        return new __SA.GetProductSalesArrangementIdResponse
        {
            SalesArrangementId = sa.SalesArrangementId
        };
    }

    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly ICodebookServiceClient _codebookService;

    public GetProductSalesArrangementIdHandler(Database.SalesArrangementServiceDbContext dbContext, ICodebookServiceClient codebookService) 
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
