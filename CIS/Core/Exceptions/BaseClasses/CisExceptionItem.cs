using System.Globalization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Instance jednotlive chyby
/// </summary>
/// <param name="ExceptionCode">CIS error kód</param>
/// <param name="Message">chybová zpráva</param>
public record CisExceptionItem(string ExceptionCode, string Message)
{
    public CisExceptionItem(int exceptionCode, string message)
        : this(exceptionCode.ToString(CultureInfo.InvariantCulture), message) 
    { }
}
