namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class KafkaConfiguration
{
    public KafkaConnectionStrings ConnectionStrings { get; set; } = default!;
}

public class KafkaConnectionStrings
{
    public string Application { get; set; } = string.Empty;
    
    public string Logging { get; set; } = string.Empty;
}