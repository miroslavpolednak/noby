namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.SyncDatamartRefixation;

public class SyncDatamartRefixationConfiguration
{
    public int BatchSize { get; set; } = 20000;
}
