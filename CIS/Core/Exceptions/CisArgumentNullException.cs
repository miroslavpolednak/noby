using System.Runtime.Serialization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Stejná chyba jako <see cref="System.ArgumentNullException"/>, ale obsahuje navíc CIS error kód
/// </summary>
[Serializable]
public sealed class CisArgumentNullException 
    : ArgumentNullException
{
    /// <summary>
    /// CIS error kód
    /// </summary>
    public int ExceptionCode { get; init; }

    private CisArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <param name="exceptionCode">CIS error kód</param>
    /// <param name="message">Chybová zpráva</param>
    /// <param name="paramName">Název parametru, který chybu vyvolal</param>
    public CisArgumentNullException(int exceptionCode, string message, string paramName)
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
