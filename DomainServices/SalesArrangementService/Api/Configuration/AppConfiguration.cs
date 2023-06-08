namespace DomainServices.SalesArrangementService.Api.Configuration;

internal sealed class AppConfiguration
{
    public string StarbuildWorkflowProcessEventTopic { get; set; } = null!;

    public void Validate()
    {
        if (string.IsNullOrEmpty(StarbuildWorkflowProcessEventTopic))
        {
            throw new CisConfigurationException(0, $"{nameof(StarbuildWorkflowProcessEventTopic)} is empty.");
        }
    }
}
