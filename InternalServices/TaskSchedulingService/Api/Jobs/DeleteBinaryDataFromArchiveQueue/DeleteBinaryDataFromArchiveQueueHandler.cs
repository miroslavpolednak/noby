using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.DocumentArchiveService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.DeleteBinaryDataFromArchiveQueue;

internal sealed class DeleteBinaryDataFromArchiveQueueHandler(IMaintananceService maintanance) : IJob
{
    private readonly IMaintananceService _maintanance = maintanance;

    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        await _maintanance.DeleteBinDataFromArchiveQueue(cancellationToken);
    }
}
