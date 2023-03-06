namespace Mock.Mcs.Configuration;

public class AppConfiguration
{
    public KafkaTopics KafkaTopics { get; set; } = null!;
}

public class KafkaTopics
{
    public string McsResult { get; set; } = null!;
    
    public string McsSender { get; set; } = null!;
}