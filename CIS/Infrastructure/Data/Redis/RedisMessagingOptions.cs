namespace CIS.Infrastructure.Data.Redis;

public class RedisMessagingOptions
{
    public const string SectionName = " RedisMessaging";

    public string RedisQueueId { get; set; } = "noby_queue";

    public string RedisChannel { get; set; } = "noby_channel";
}
