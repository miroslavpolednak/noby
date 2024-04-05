using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.OfferService.Clients.Interfaces;


namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.SyncDatamartRefixation;

public class SyncDatamartRefixationHandler(IMaintananceService maintanance) : IJob
{
    private readonly IMaintananceService _maintanance = maintanance;

    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var configuration = System.Text.Json.JsonSerializer.Deserialize<SyncDatamartRefixationConfiguration>(jobData ?? "{}");

        await _maintanance.ImportOfferFromDatamart(new() { BatchSize = configuration?.BatchSize ?? 10000 }, cancellationToken);
    }
}
