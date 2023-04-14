namespace CIS.Infrastructure.Messaging.Exceptions;

public class KafkaMessageTypeNotSupportedException : Exception
{
    public string Type { get; }

    public KafkaMessageTypeNotSupportedException(string type)
    {
        Type = type;
    }
}