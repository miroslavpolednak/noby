namespace DomainServices.CaseService.Api.Configuration;

internal sealed class AppConfiguration
{
    public string? SbWorkflowProcessTopic { get; set; }

    public string? SbWorkflowInputProcessingTopic { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(SbWorkflowProcessTopic))
        {
            throw new CisConfigurationException(0, $"{nameof(SbWorkflowProcessTopic)} Kafka topic is empty");
        }
        if (string.IsNullOrEmpty(SbWorkflowInputProcessingTopic))
        {
            throw new CisConfigurationException(0, $"{nameof(SbWorkflowInputProcessingTopic)} Kafka topic is empty");
        }
    }
}
