using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance.GetCancelServiceSalesArrangementsIds;

internal sealed class GetCancelServiceSalesArrangementsIdsHandler(
    SalesArrangementServiceDbContext _dbContext, 
    ICodebookServiceClient _codebookService, 
    TimeProvider _dateTimeService)
		: IRequestHandler<GetCancelServiceSalesArrangementsIdsRequest, GetCancelServiceSalesArrangementsIdsResponse>
{
    public async Task<GetCancelServiceSalesArrangementsIdsResponse> Handle(GetCancelServiceSalesArrangementsIdsRequest request, CancellationToken cancellationToken)
    {
        var salesArrangementTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .Where(s => s.SalesArrangementCategory == 2 && !_notAllowedSaTypes.Contains(s.Id))
            .Select(t => t.Id);

        var saIdsForDelete = await _dbContext.SalesArrangements.Where(s =>
            salesArrangementTypes.Contains(s.SalesArrangementTypeId)
            && s.FirstSignatureDate == null 
            && s.CreatedTime < _dateTimeService.GetLocalNow().AddDays(-90)
            )
            .Select(sa => sa.SalesArrangementId)
            .ToListAsync(cancellationToken);

        var response = new GetCancelServiceSalesArrangementsIdsResponse();
        response.SalesArrangementId.AddRange(saIdsForDelete);
        return response;
    }

    private static readonly int[] _notAllowedSaTypes = [13,14,15];
}
