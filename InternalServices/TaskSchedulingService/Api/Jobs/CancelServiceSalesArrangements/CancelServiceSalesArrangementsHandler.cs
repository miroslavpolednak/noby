using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelServiceSalesArrangements;

/// <summary>
/// Original SalesArrangement.CancelServiceSalesArrangementsHandler
/// </summary>
internal sealed class CancelServiceSalesArrangementsHandler(
    ISalesArrangementServiceClient _salesArrangementService, 
    ILogger<CancelServiceSalesArrangementsHandler> _logger, 
    DomainServices.SalesArrangementService.Clients.IMaintananceService _maintananceService)
        : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var saIdsForDelete = await _maintananceService.GetCancelServiceSalesArrangementsIds(cancellationToken);

        _logger.DeleteServiceSalesArrangement(saIdsForDelete.Length);

        foreach (int saId in saIdsForDelete)
        {
            await _salesArrangementService.DeleteSalesArrangement(saId, true, cancellationToken);
        }
    }
}
