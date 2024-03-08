using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance.GetCancelServiceSalesArrangementsIds;

internal sealed class GetCancelServiceSalesArrangementsIdsHandler
    : IRequestHandler<GetCancelServiceSalesArrangementsIdsRequest, GetCancelServiceSalesArrangementsIdsResponse>
{
    public async Task<GetCancelServiceSalesArrangementsIdsResponse> Handle(GetCancelServiceSalesArrangementsIdsRequest request, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = (await _codebookService.SalesArrangementTypes(cancellationToken)).Where(s => s.SalesArrangementCategory == 2);

        var saIdsForDelete = await _dbContext.SalesArrangements.Where(s =>
            salesArrangementTypes.Select(r => r.Id).Contains(s.SalesArrangementTypeId)
            &&
              (
                (s.FirstSignatureDate == null && s.CreatedTime < _dateTimeService.GetLocalNow().AddDays(-90))
                ||
                (s.FirstSignatureDate != null && s.FirstSignatureDate < _dateTimeService.GetLocalNow().AddDays(-40) && s.State != 2) // State = 2 (Předáno ke zpracování)
              )
            )
            .Select(sa => sa.SalesArrangementId)
            .ToListAsync(cancellationToken);

        var response = new GetCancelServiceSalesArrangementsIdsResponse();
        response.SalesArrangementId.AddRange(saIdsForDelete);
        return response;
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly TimeProvider _dateTimeService;

    public GetCancelServiceSalesArrangementsIdsHandler(SalesArrangementServiceDbContext dbContext, ICodebookServiceClient codebookService, TimeProvider dateTimeService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
        _dateTimeService = dateTimeService;
    }
}
