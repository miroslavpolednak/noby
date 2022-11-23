namespace CIS.Core.Exceptions;

/// <summary>
/// Doménová nebo infrastrukturní služba není k dispozici - např. špatné URL volané služby, nebo volaná služba vůbec neběží.
/// </summary>
public sealed class CisServiceUnavailableException 
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
    public CisServiceUnavailableException(string serviceName, string methodName, string message) 
        : base(5, message) 
    {
        MethodName = methodName;
        ServiceName = serviceName;
    }
}
