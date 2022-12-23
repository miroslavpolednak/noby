namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class AppConfiguration
{
    public KafkaTopics KafkaTopics { get; set; } = null!;
}

public class KafkaTopics
{
    public string McsResult { get; set; } = null!;
    
    public string McsSender { get; set; } = null!;

    public string NobyResult { get; set; } = null!;

    public string NobySendEmail { get; set; } = null!;
}