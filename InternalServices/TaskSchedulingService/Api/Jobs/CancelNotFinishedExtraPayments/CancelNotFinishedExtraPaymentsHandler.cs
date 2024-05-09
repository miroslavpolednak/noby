using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelNotFinishedExtraPayments;

public class CancelNotFinishedExtraPaymentsHandler(ISalesArrangementServiceClient salesArrangementService) : IJob
{
    private readonly ISalesArrangementServiceClient _salesArrangementService = salesArrangementService;

    public Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
