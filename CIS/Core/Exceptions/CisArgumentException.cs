using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

[Serializable]
public sealed class CisArgumentException : ArgumentException
{
    public int ExceptionCode { get; init; }

    public CisArgumentException(int exceptionCode, string? message, string? paramName)
        : base(message, paramName)
    {
        this.ExceptionCode = exceptionCode;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ExceptionCode), ExceptionCode, typeof(int));
    }
}
