using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.SalesArrangementService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelNotFinishedExtraPayments;

public class CancelNotFinishedExtraPaymentsHandler(IMaintananceService _maintanance) 
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        await _maintanance.CancelNotFinishedExtraPayments(cancellationToken);
    }
}
