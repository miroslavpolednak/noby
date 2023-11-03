using CIS.Core.Exceptions;

namespace CIS.Infrastructure.Messaging.Exceptions;

public sealed class KafkaMessageTypeNotSupportedException 
    : Exception, ICisExceptionExludedFromLog
{
    public string Type { get; }

    public KafkaMessageTypeNotSupportedException(string type)
    {
        Type = type;
    }
}