namespace DomainServices.CaseService.Api.Configuration;

internal sealed class AppConfiguration
{
    public string? MainLoanProcessChangedTopic { get; set; }

    public string? CaseStateChangedProcessingCompletedTopic { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(MainLoanProcessChangedTopic))
        {
            throw new CisConfigurationException(0, "MainLoanProcessChangedTopic Kafka topic is empty");
        }
        if (string.IsNullOrEmpty(CaseStateChangedProcessingCompletedTopic))
        {
            throw new CisConfigurationException(0, "CaseStateChangedProcessingCompletedTopic Kafka topic is empty");
        }
    }
}
