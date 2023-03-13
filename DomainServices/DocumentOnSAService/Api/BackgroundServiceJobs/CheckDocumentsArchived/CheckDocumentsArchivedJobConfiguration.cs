using CIS.Infrastructure.BackgroundServiceJob;

namespace DomainServices.DocumentOnSAService.Api.BackgroundServiceJobs.CheckDocumentsArchived;

public class CheckDocumentsArchivedJobConfiguration : IPeriodicJobConfiguration<CheckDocumentsArchivedJob>
{
    public string SectionName => "CheckDocumentsArchivedJobConfiguration";

    public bool ServiceDisabled { get; set; }

    public TimeSpan TickInterval { get; set; } = TimeSpan.FromMinutes(1); //Dafault

    public short MaxBatchSize { get; set; } = 1000;

}
