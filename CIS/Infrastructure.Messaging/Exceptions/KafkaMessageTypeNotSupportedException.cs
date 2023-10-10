using CIS.Core.Exceptions;

namespace CIS.Infrastructure.Messaging.Exceptions;

public class KafkaMessageTypeNotSupportedException : Exception, ICisLogExcludeException
{
    public string Type { get; }

    public KafkaMessageTypeNotSupportedException(string type)
    {
        Type = type;
    }
}