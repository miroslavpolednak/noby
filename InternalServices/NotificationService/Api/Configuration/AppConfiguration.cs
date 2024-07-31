namespace CIS.InternalServices.NotificationService.Api.Configuration;

internal sealed class AppConfiguration
{
    public List<Consumer> Consumers { get; set; } = new();

    public EmailSenders EmailSenders { get; set; } = null!;

    [Obsolete]
    public List<string> EmailFormats { get; set; } = new();

    [Obsolete]
    public List<string> EmailLanguageCodes { get; set; } = new();
    
    public KafkaTopics KafkaTopics { get; set; } = null!;

    public RateLimit RateLimit { get; set; } = null!;
}

internal sealed class Consumer
{
    public string Username { get; set; } = null!;

    public string ConsumerId { get; set; } = null!;
}

internal sealed class EmailSenders
{
    public List<string> Mcs { get; set; } = new();

    public List<string> Mpss { get; set; } = new();

    public Dictionary<string, string> AddressMapping { get; set; } = new();
}

internal sealed class KafkaTopics
{
    public string McsResult { get; set; } = null!;
    
    public string McsSender { get; set; } = null!;

    public static string McsIdPrefix { get => "NOBYNS-"; }
}

internal sealed class RateLimit
{
    public int PermitLimit { get; set; }

    public int WindowDurationInSeconds { get; set; }
}
