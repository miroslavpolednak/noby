using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

public class CisConflictException : BaseCisException
{
    public IReadOnlyCollection<(string Key, string Message)>? Errors { get; init; }

    public CisConflictException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    {
    }

    public CisConflictException(int exceptionCode, string message)
        : base(exceptionCode, message)
    {
    }

    public CisConflictException(IEnumerable<(string Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public CisConflictException(IEnumerable<(int Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        Errors = errors.Select(t => (t.Key.ToString(System.Globalization.CultureInfo.InvariantCulture), t.Message)).ToList().AsReadOnly();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        if (Errors is not null)
            info.AddValue(nameof(Errors), Errors, typeof(List<(string Key, string Message)>));
    }
}
