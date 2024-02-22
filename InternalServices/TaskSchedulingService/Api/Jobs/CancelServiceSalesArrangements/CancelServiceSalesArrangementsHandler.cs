using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelServiceSalesArrangements;

internal sealed class CancelServiceSalesArrangementsHandler
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

    private readonly ILogger<CancelServiceSalesArrangementsHandler> _logger;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.SalesArrangementService.Clients.IMaintananceService _maintananceService;

    public CancelServiceSalesArrangementsHandler(ISalesArrangementServiceClient salesArrangementService, ILogger<CancelServiceSalesArrangementsHandler> logger, DomainServices.SalesArrangementService.Clients.IMaintananceService maintananceService)
    {
        _salesArrangementService = salesArrangementService;
        _logger = logger;
        _maintananceService = maintananceService;
    }
}
