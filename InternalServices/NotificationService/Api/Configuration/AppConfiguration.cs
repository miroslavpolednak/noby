namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class AppConfiguration
{
    public Dictionary<string, string> UserConsumerIdMap { get; set; } = new();
    
    public EmailSenders EmailSenders { get; set; } = null!;
    
    public KafkaTopics KafkaTopics { get; set; } = null!;

    public S3Buckets S3Buckets { get; set; } = null!;
}

public class EmailSenders
{
    public HashSet<string> Mcs { get; set; } = new();

    public HashSet<string> Mpss { get; set; } = new();
}

public class KafkaTopics
{
    public string McsResult { get; set; } = null!;
    
    public string McsSender { get; set; } = null!;

    public string NobyResult { get; set; } = null!;

    public string NobySendEmail { get; set; } = null!;
}

public class S3Buckets
{
    public string Mcs { get; set; } = null!;

    public string Mpss { get; set; } = null!;
}