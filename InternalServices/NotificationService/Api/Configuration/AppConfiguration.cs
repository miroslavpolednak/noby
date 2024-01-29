namespace CIS.InternalServices.NotificationService.Api.Configuration;

internal sealed class AppConfiguration
{
    public List<Consumer> Consumers { get; set; } = new();

    public EmailSenders EmailSenders { get; set; } = null!;

    public HashSet<string> EmailFormats { get; set; } = new();
    
    public HashSet<string> EmailLanguageCodes { get; set; } = new();
    
    public KafkaTopics KafkaTopics { get; set; } = null!;

    public S3Buckets S3Buckets { get; set; } = null!;
}

internal sealed class Consumer
{
    public string Username { get; set; } = null!;

    public string ConsumerId { get; set; } = null!;

    public bool CanSendEmail { get; set; }
    
    public bool CanSendSms { get; set; }
    
    public bool CanReadResult { get; set; }

    public bool CanReceiveStatistics { get; set; }

    public bool CanResendNotifications { get; set; }
}

internal sealed class EmailSenders
{
    public HashSet<string> Mcs { get; set; } = new();

    public HashSet<string> Mpss { get; set; } = new();
}

internal sealed class KafkaTopics
{
    public string McsResult { get; set; } = null!;
    
    public string McsSender { get; set; } = null!;
}

internal sealed class S3Buckets
{
    public string Mcs { get; set; } = null!;
}