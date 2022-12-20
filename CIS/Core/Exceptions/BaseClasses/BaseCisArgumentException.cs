using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Stejná chyba jako <see cref="System.ArgumentException"/>, ale obsahuje navíc CIS error kód
/// </summary>
[Serializable]
public abstract class BaseCisArgumentException 
    : ArgumentException
{
    /// <summary>
    /// CIS error kód
    /// </summary>
    public int ExceptionCode { get; protected set; }

    protected BaseCisArgumentException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    /// <param name="paramName">Název parametru, který chybu vyvolal</param>
    protected BaseCisArgumentException(int exceptionCode, string message, string paramName)
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
