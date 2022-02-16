using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

public class CisValidationException : BaseCisException
{
    public IReadOnlyCollection<(string Key, string Message)>? Errors { get; init; }

    public bool ContainErrorsList
        => Errors != null && Errors.Any();

    public CisValidationException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    {
    }

    public CisValidationException(int exceptionCode, string message) 
        : base(exceptionCode, message)
    {
    }

    public CisValidationException(CisArgumentException exception) 
        : base(exception.ExceptionCode, exception.Message)
    {
    }
    
    public CisValidationException(IEnumerable<(string Key, string Message)> errors, string message = "") 
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public CisValidationException(IEnumerable<(int Key, string Message)> errors, string message = "")
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
