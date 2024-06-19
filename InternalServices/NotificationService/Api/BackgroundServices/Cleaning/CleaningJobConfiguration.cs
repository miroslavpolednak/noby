namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.Cleaning;

internal sealed class CleaningJobConfiguration
{
    public int CleanInProgressInMinutes { get; set; }
    public int EmailSlaInMinutes { get; set; }
}
