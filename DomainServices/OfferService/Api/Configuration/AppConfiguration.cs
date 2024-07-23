namespace DomainServices.OfferService.Api.Configuration;

public class AppConfiguration
{
    public string? SbWorkflowProcessTopic { get; set; }

    
    public void Validate()
    {
        if (string.IsNullOrEmpty(SbWorkflowProcessTopic))
        {
            throw new CisConfigurationException(0, $"{nameof(SbWorkflowProcessTopic)} Kafka topic is empty");
        }
    }
}