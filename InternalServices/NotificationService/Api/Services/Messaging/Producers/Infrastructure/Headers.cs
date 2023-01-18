namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;

public class Headers
{
    public string Id { get; set; } = null!;
 
    public string? B3 { get; set; }

    public string ReplyTopic { get; set; } = null!;

    public string ReplyBrokerUri { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Origin { get; set; } = null!;

    public string Caller { get; set; } = null!;
}