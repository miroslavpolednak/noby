namespace CIS.Core.Exceptions;

/// <summary>
/// HTTP 500. Vyhazuje se pokud naše doménová nebo infrastrkuturní služba vrátí server error - 500.
/// </summary>
public sealed class CisServiceServerErrorException 
    : BaseCisException
{
    /// <summary>
    /// Název služby, která selhala
    /// </summary>
    public string ServiceName { get; init; }

    /// <summary>
    /// Metoda / endpoint jehož volání selhalo
    /// </summary>
    public string MethodName { get; init; }

    /// <param name="serviceName">Název služby, která selhala</param>
    /// <param name="methodName">Metoda / endpoint jehož volání selhalo</param>
    /// <param name="message">Textový popis chyby</param>
    public CisServiceServerErrorException(string serviceName, string methodName, string message)
        : base(15, message)
    {
        MethodName = methodName;
        ServiceName = serviceName;
    }
}
