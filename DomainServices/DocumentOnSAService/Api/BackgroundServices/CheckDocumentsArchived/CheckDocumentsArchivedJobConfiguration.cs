namespace DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;

public sealed class CheckDocumentsArchivedJobConfiguration
{
    public short MaxBatchSize { get; set; } = 1000;
}
