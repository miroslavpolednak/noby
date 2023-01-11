using System.Globalization;

namespace CIS.Core.Exceptions;

/// <summary>
/// Instance jednotlive chyby
/// </summary>
/// <param name="Code">CIS error kód</param>
/// <param name="Message">chybová zpráva</param>
public record CisExceptionItem(string Code, string Message)
{
    public CisExceptionItem(int code, string message)
        : this(code.ToString(CultureInfo.InvariantCulture), message) 
    { }
}
