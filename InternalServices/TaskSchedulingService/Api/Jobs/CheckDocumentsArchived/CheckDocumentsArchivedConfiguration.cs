namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CheckDocumentsArchived;

internal sealed class CheckDocumentsArchivedConfiguration
{
    public short MaxBatchSize { get; set; } = 1000;
}
