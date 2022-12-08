using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Base třída pro CIS vyjímky. Obsahuje vlastnost ExceptionCode, která určuje o jakou vyjímku se jedná.
/// </summary>
[Serializable]
public abstract class BaseCisException 
    : Exception
{
    /// <summary>
    /// Společný kód pro neznámou chybu
    /// </summary>
    public const int UnknownExceptionCode = 0;

    /// <summary>
    /// CIS error kód
    /// </summary>
    public int ExceptionCode { get; protected set; }

    protected BaseCisException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    public BaseCisException(int exceptionCode, string? message)
        : base(message)
    {
        this.ExceptionCode = exceptionCode;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ExceptionCode), ExceptionCode, typeof(int));
    }
}
