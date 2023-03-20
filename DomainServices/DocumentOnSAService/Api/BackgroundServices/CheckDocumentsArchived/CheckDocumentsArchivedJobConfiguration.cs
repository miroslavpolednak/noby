namespace DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;

internal sealed class CheckDocumentsArchivedJobConfiguration
{
    public short MaxBatchSize { get; set; } = 1000;
}
