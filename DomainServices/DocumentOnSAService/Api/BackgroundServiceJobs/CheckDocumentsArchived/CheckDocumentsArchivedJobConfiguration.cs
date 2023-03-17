namespace DomainServices.DocumentOnSAService.Api.BackgroundServiceJobs.CheckDocumentsArchived;

internal sealed class CheckDocumentsArchivedJobConfiguration
{
    public short MaxBatchSize { get; set; } = 1000;
}
